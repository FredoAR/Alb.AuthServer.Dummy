using Alb.AuthServer.Domain.Models.Identity;
using Alb.AuthServer.Domain.Shared.Enums;
using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;


namespace Alb.AuthServer.Infrastructure.Extensions
{
    public static class IdentityExtension
    {
        public static AuthIdentityUser MapToAuthIdentityUser(this AuthCreateUserModel dto)
        {
            RegistrationSources origin = (RegistrationSources)dto.RegistrationSource;

            return new AuthIdentityUser
            {
                /* El email sera tomado como UserName, si requiero nombre creare una tabla perfil.nombre */
                //UserName = dto.UserName,
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsInternalClient = dto.IsInternalClient,
                RegistrationSource = origin,
                RegistrationSourceDisplay = $"{origin}"                
            };
        }


        public static AuthIdentityErrorModel[] MapToAuthIdentityErrorModel(this IEnumerable<IdentityError> errors)
        {
            var authErrors = new List<AuthIdentityErrorModel>();

            foreach (var error in errors)
            {
                authErrors.Add(new AuthIdentityErrorModel { Code = error.Code, Description = error.Description });
            }

            return authErrors.ToArray();
        }


        public static TokenResultModel MapToTokenResultModel(this JwtSecurityToken token)
        {
            return new TokenResultModel
            {
                TokenType = "Bearer",
                ExpiresOn = $"{new DateTimeOffset(token.ValidTo).ToUnixTimeSeconds()}",
                //NotBefore = $"{new DateTimeOffset(token.ValidFrom).ToUnixTimeSeconds()}"
            };
        }



    }
}
