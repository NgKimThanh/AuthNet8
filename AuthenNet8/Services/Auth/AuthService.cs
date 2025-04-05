using AuthenNet8.DTO.Auth;
using AuthenNet8.DTO.SYS;
using AuthenNet8.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthenNet8.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DBContext dbContext, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Đăng ký
        [HttpPost("register")]
        public async Task<SYSUser> Auth_Register(RegisterRequest request)
        {
            var result = new SYSUser();

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = await _dbContext.SYS_User.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (user != null)
            {
                throw new Exception("Email already exists");
            }
            else
            {
                user = new SYS_User
                {
                    Email = request.Email,
                    Password = request.Password,
                    LastName = request.LastName,
                    PasswordHash = Convert.ToBase64String(passwordHash),
                    PasswordSalt = Convert.ToBase64String(passwordSalt)
                };
                _dbContext.SYS_User?.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            result.Email = user.Email;
            return result;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion Đăng ký

        #region Đăng nhập
        public async Task<string> Auth_Login(LoginRequest rqUser)
        {
            string token = "";
            #region Verify user
            // Lấy thông tin user
            var user = await _dbContext.SYS_User.FirstOrDefaultAsync(c => c.Email == rqUser.Email);
            if (user == null || user.Email != rqUser.Email)
                throw new Exception("User not found.");
            if (string.IsNullOrEmpty(user.PasswordHash))
                throw new Exception("PasswordHash error.");
            if (string.IsNullOrEmpty(user.PasswordSalt))
                throw new Exception("PasswordSalt error.");

            // Lấy passwordHash & passwordSalt từ user
            var passwordHash = Convert.FromBase64String(user.PasswordHash);
            var passwordSalt = Convert.FromBase64String(user.PasswordSalt);

            // Verify Password từ passwordHash & passwordSalt
            if (!VerifyPasswordHash(rqUser.Password, passwordHash, passwordSalt))
                throw new Exception("Wrong password.");
            #endregion

            #region Tạo token
            // Tạo token
            token = CreateToken(user);
            // Tạo refresh token
            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user.ID);

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;
            await _dbContext.SaveChangesAsync();
            #endregion

            return token;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        #endregion Đăng nhập

        #region Refresh token
        public async Task<string> Auth_RefreshToken()
        {
            string refreshToken = string.Empty;
            var userID = -1;
            try
            {
                refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"] ?? string.Empty;
                // Có thể lưu userID này ở local storage để tránh bị js đánh cắp token
                userID = Convert.ToInt32(_httpContextAccessor.HttpContext!.Request.Cookies["userId"]);
            }
            catch
            {
                throw new Exception("No Refresh Token found.");
            }

            if (string.IsNullOrEmpty(refreshToken) || userID < 0)
                throw new Exception("No Refresh Token found.");

            // Lấy user từ HpptOnly Cookie userID
            var user = await _dbContext.SYS_User.FirstOrDefaultAsync(c => c.ID == userID);
            if (user == null)
                throw new Exception("Invalid user.");

            if (user.TokenExpires < DateTime.UtcNow)
                throw new Exception("Refresh Token expired.");

            string newToken = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user.ID);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
            await _dbContext.SaveChangesAsync();

            return newToken;
        }
        #endregion Refresh token

        #region Đăng xuất
        public async Task Auth_Logout()
        {
            // Lấy refresh token từ cookie
            var refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                throw new Exception("No Refresh Token found.");

            // Tìm user có refresh token này
            var user = await _dbContext.SYS_User.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
                throw new Exception("Invalid Refresh Token.");

            // Xóa refresh token trong database
            user.RefreshToken = null;
            user.TokenCreated = null;
            user.TokenExpires = null;
            await _dbContext.SaveChangesAsync();

            // Xóa cookie refresh token
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("refreshToken");
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("userID");
        }
        #endregion Đăng xuất

        private string CreateToken(SYS_User user)
        {
            // Tạo ra danh sách các quyền có trong 1 token
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Role, user.Role)
            };

            // Tạo 1 key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:SecretKey").Value));

            // Ký mã jwt bằng thuật toán SecurityAlgorithms.HmacSha512Signature
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Tạo mã jwt
            var token = new JwtSecurityToken(
                claims: claims, // Danh sách quyền
                expires: DateTime.Now.AddDays(1), // Thời hạn
                signingCredentials: creds // Ký mã jwt
                );

            // Tạo chuỗi jwt
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7).ToUniversalTime(),
                Created = DateTime.Now.ToUniversalTime()
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, int userID)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("userID", userID.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = newRefreshToken.Expires
            });
        }
    }
}
