using Alb.AuthServer.Application.Contracts.Common;
using Alb.AuthServer.Application.Contracts.Identity.Dto;
using Alb.AuthServer.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alb.AuthServer.Application.Extensions
{
    public static class IdenttityExtension
    {
        public static AuthError[] MapToAuthError(this IEnumerable<AuthIdentityErrorModel> errorsModel)
        {
            var authErrors = new List<AuthError>();

            foreach (var error in errorsModel)
            {
                authErrors.Add(new AuthError { Code = error.Code, Description = error.Description });
            }

            return authErrors.ToArray();
        }


        public static TokenDto MapToTokenDto(this TokenResultModel tokenModel)
        {
            return new TokenDto
            {
                TokenType = tokenModel.TokenType,
                ExpiresIn = tokenModel.ExpiresIn,
                ExpiresOn = tokenModel.ExpiresOn,
                //NotBefore = tokenModel.NotBefore,
                AccessToken = tokenModel.AccessToken,
            };
        }
    }
}
