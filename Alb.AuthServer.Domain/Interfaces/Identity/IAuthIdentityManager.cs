using Alb.AuthServer.Domain.Models.Identity;


namespace Alb.AuthServer.Domain.Interfaces.Identity
{
    // Domain Service
    public interface IAuthIdentityManager
    {        
        Task<bool> FindUserByEmailAsync(string email);

        Task<AuthIdentityResultModel> RegisterAsync(AuthCreateUserModel model, bool isAdmin);

        Task<AuthIdentityResultModel<TokenResultModel>> LoginAsync(AuthLoginModel model);

        Task<AuthIdentityResultModel<TokenResultModel>> RefreshTokenAsync(AuthLoginModel model);
    }
}
