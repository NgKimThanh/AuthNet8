using AuthenNet8.DTO.Auth;
using AuthenNet8.DTO.SYS;

namespace AuthenNet8.Services.Auth
{
    public interface IAuthService
    {
        Task<SYSUser> Auth_Register(RegisterRequest request);

        Task<string> Auth_Login(LoginRequest rqUser);

        Task<string> Auth_RefreshToken();

        Task Auth_Logout();
    }
}
