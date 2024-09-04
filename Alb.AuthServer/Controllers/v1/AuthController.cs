using Alb.AuthServer.Application.Contracts.Identity;
using Alb.AuthServer.Application.Contracts.Identity.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;



namespace Alb.Identity.Controllers.v1
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityAppService IdentityAppService;
        private readonly ILogger<AuthController> Logger;
                

        /// <summary>
        /// Constructor
        /// </summary>        
        public AuthController(IIdentityAppService identityAppService, ILogger<AuthController> logger)            
        {
            IdentityAppService = identityAppService;
            Logger = logger;                        
        }
               

        // api/v1/auth/register
        [Authorize]
        [HttpPost("register", Name = "UserRegister")]        
        public async Task<IActionResult> UserRegisterAsync([FromBody] CreateUserDto dto)
        {
            try
            {                
                if (!ModelState.IsValid)
                    return BadRequest(dto);
                
                var userResponseDto = await IdentityAppService.RegisterAsync(dto);

                if(userResponseDto.Succeeded)
                    return Ok(userResponseDto);

                return BadRequest(userResponseDto);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error {nameof(UserRegisterAsync)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Ocurrio un error interno, alb");
            }
        }


        // api/v1/auth/register/admin
        [Authorize]
        [HttpPost("register/admin", Name = "AdminRegister")]
        public async Task<IActionResult> AdminRegisterAsync([FromBody] CreateUserDto dto)
        {
            try
            {                
                if (!ModelState.IsValid)
                    return BadRequest(dto);
                
                var userResponseDto = await IdentityAppService.RegisterAsync(dto, true);

                if(userResponseDto.Succeeded)
                    return Ok(userResponseDto);

                return BadRequest(userResponseDto);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error {nameof(AdminRegisterAsync)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Ocurrio un error interno, alb");
            }
        }


        // api/v1/auth/login
        [AllowAnonymous]
        [HttpPost("login", Name = "Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto dto)
        {
            try
            {                                
                var tokenResponseDto = await IdentityAppService.LoginAsync(dto);
                
                if(tokenResponseDto.Succeeded)
                    return Ok(tokenResponseDto);                

                return BadRequest(tokenResponseDto);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error {nameof(LoginAsync)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Ocurrio un error interno, alb");
            }
        }


        // api/v1/refresh-token        
        [HttpGet("refresh-token", Name = "RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {                
                var emailJwtType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == emailJwtType).FirstOrDefault();
                
                var loginDto = new LoginUserDto
                {
                    Email = emailClaim?.Value,
                    Password = string.Empty
                };

                var tokenResultDto = await IdentityAppService.RefreshTokenAsync(loginDto);
                
                if(tokenResultDto.Succeeded)
                    return Ok(tokenResultDto);

                return BadRequest(tokenResultDto);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error {nameof(RefreshToken)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Ocurrio un error interno, alb");
            }

        }       


    }
}
