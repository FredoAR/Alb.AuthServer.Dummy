using Alb.AuthServer.Application.Contracts;
using Alb.AuthServer.Application.Contracts.Identity;
using Alb.AuthServer.Application.Contracts.Identity.Dto;
using Alb.AuthServer.Application.Extensions;
using Alb.AuthServer.Domain.Interfaces.Identity;
using Alb.AuthServer.Domain.Models.Identity;
using AutoMapper;

namespace Alb.AuthServer.Application.Identity
{
    public class IdentityAppService : IIdentityAppService
    {       
        private readonly IAuthIdentityManager AuthIdentityMngIntegration;
        private readonly IMapper Mapper;
        

        /// <summary>
        /// Constructor
        /// </summary>
        public IdentityAppService(IAuthIdentityManager authIdentityIntegration, IMapper mapper)
        {           
            AuthIdentityMngIntegration = authIdentityIntegration;
            Mapper = mapper;           
        }        


        /// <summary>
        /// Registrar usuarios
        /// </summary>
        public async Task<CreateUserResponseDto> RegisterAsync(CreateUserDto dto, bool isAdmin = false)
        {
            try
            {
                /* user exist by email */
                var userExist = await AuthIdentityMngIntegration.FindUserByEmailAsync(dto.Email);
                if (userExist is true)                    
                    return CreateUserResponseDto.Failed(AuthErrorCodes.Alb001);
                
                /* convert dto to domain model */
                var userModel = Mapper.Map<AuthCreateUserModel>(dto);

                /* model to domain service: AuthIdentityResultModel */
                var result = await AuthIdentityMngIntegration.RegisterAsync(userModel, isAdmin);
                
                if(!result.Succeeded)
                {
                    var errors = result.Errors.MapToAuthError();
                    return CreateUserResponseDto.Failed(errors);
                }

                return CreateUserResponseDto.Success;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Autenticación y generación de token
        /// </summary>        
        public async Task<TokenResultDto> LoginAsync(LoginUserDto dto)
        {
            try
            {
                var loginModel = Mapper.Map<AuthLoginModel>(dto);
                
                var userLogin = await AuthIdentityMngIntegration.LoginAsync(loginModel);
                if(!userLogin.Succeeded)
                {
                    var errors = userLogin.Errors.MapToAuthError();
                    return TokenResultDto.Failed(errors);
                }               

                var tokenResultDto = TokenResultDto.Success;
                tokenResultDto.Token = userLogin.Data.MapToTokenDto();
                return tokenResultDto;
            }
            catch (Exception ex)
            {
                throw;
            }                                   
        }


        public async Task<TokenResultDto> RefreshTokenAsync(LoginUserDto dto)
        {
            try
            {
                var loginModel = Mapper.Map<AuthLoginModel>(dto);
                
                var userLogin = await AuthIdentityMngIntegration.RefreshTokenAsync(loginModel);
                if (!userLogin.Succeeded)
                {
                    var errors = userLogin.Errors.MapToAuthError();
                    return TokenResultDto.Failed(errors);
                }

                var tokenResultDto = TokenResultDto.Success;
                tokenResultDto.Token = userLogin.Data.MapToTokenDto();
                return tokenResultDto;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
