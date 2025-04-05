using AuthenNet8.DTO.Auth;
using AuthenNet8.DTO.SYS;
using AuthenNet8.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthenNet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Đăng ký
        [HttpPost("register")]
        public async Task<IActionResult> Auth_Register([FromBody] RegisterRequest request)
        {
            return Ok(await _authService.Auth_Register(request));
        }
        #endregion Đăng ký

        #region Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> Auth_Login([FromBody] LoginRequest request)
        {
            return Ok(await _authService.Auth_Login(request));
        }
        #endregion Đăng nhập

        #region Refresh token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> Auth_RefreshToken()
        {
            return Ok(await _authService.Auth_RefreshToken());
        }
        #endregion Refresh token

        #region Đăng xuất
        [HttpPost("logout")]
        public async Task<IActionResult> Auth_Logout()
        {
            await _authService.Auth_Logout();
            return Ok();
        }
        #endregion Đăng xuất
    }
}
