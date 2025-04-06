using AuthenNet8.Entities;
using AuthenNet8.Entities.SYS;

namespace AuthenNet8.Repositories.Users
{
    public interface IUserRepository
    {
        #region User
        Task<SYS_User?> GetByEmailAsync(string email);
        Task<SYS_User?> GetByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        void AddUser(SYS_User user);
        #endregion User

        #region UserRefreshToken
        void AddUserRefreshToken(SYS_UserRefreshToken userRefreshToken);
        Task<List<SYS_UserRefreshToken>> GetUserRefreshTokensAsync(int userId);
        Task<SYS_UserRefreshToken> GetUserRefreshTokenAsync(int userId, string token);
        #endregion UserRefreshToken

        #region UserPasswordReset
        void AddUserPasswordReset(SYS_UserPasswordReset userPasswordReset);
        Task<SYS_UserPasswordReset?> GetValidResetTokenAsync(string token);
        Task RemoveUserPasswordReset(SYS_UserPasswordReset userPasswordReset);
        #endregion UserPasswordReset

        Task SaveChangesAsync();
    }
}
