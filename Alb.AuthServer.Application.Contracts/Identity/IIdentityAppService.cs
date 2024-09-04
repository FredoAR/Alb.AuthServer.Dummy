using Alb.AuthServer.Application.Contracts.Identity.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alb.AuthServer.Application.Contracts.Identity
{
    public interface IIdentityAppService
    {       
        Task<CreateUserResponseDto> RegisterAsync(CreateUserDto dto, bool isAdmin = false);

        Task<TokenResultDto> LoginAsync(LoginUserDto dto);

        Task<TokenResultDto> RefreshTokenAsync(LoginUserDto dto);



    }
}
