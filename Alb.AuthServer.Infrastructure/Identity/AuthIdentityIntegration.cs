using Alb.AuthServer.Domain.Interfaces.Identity;
using Alb.AuthServer.Domain.Models.Identity;
using Alb.AuthServer.Domain.Models.Options;
using Alb.AuthServer.Domain.Shared;
using Alb.AuthServer.Domain.Shared.Identity;
using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Entities;
using Alb.AuthServer.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Alb.AuthServer.Infrastructure.Identity
{
    /// <summary>
    /// Este servicio se creo como un puente entre la capa de infraestrucutra y la de aplicación
    /// para poder utilizar recursos sin acoplar las capas, funciona como un servicio de dominio
    /// usando la interfaz para poder abstraer las operaciones IAuthIdentityManager
    /// </summary>
    public class AuthIdentityIntegration : IAuthIdentityManager
    {
        /* para gestión (creación, actualizacióon, eliminar, asignar roles) */
        private readonly UserManager<AuthIdentityUser> UserManager;

        /* para autenticación (inicio de sesión, cerrar sesión, auth 2FA) */
        private readonly SignInManager<AuthIdentityUser> SignInManager;
        private readonly JwtOptions JwtOptions;




        public AuthIdentityIntegration(UserManager<AuthIdentityUser> userManager, SignInManager<AuthIdentityUser> signInManager, IOptions<JwtOptions> jwtOptions)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            JwtOptions = jwtOptions.Value;
        }


        public async Task<bool> FindUserByEmailAsync(string email)
        {
            var userExist = await UserManager.FindByEmailAsync(email);
            return (userExist != null);

        }


        public async Task<AuthIdentityResultModel> RegisterAsync(AuthCreateUserModel dto, bool isAdmin)
        {
            try
            {
                var authIdentityUser = dto.MapToAuthIdentityUser();
                var resultCreateUser = await UserManager.CreateAsync(authIdentityUser, dto.Password);
                
                if (!resultCreateUser.Succeeded)
                {
                    var mapErrors = resultCreateUser.Errors.MapToAuthIdentityErrorModel();
                    return AuthIdentityResultModel.Failed(mapErrors);
                }

                await AddRolesToUser(authIdentityUser, isAdmin);

                return AuthIdentityResultModel.Success;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        
        /// <summary>
        /// Login de usuarios
        /// </summary>
        public async Task<AuthIdentityResultModel<TokenResultModel>> LoginAsync(AuthLoginModel model)
        {
            var identityUser = await UserManager.FindByEmailAsync(model.Email);
            if (identityUser == null)
            {
                return AuthIdentityResultModel<TokenResultModel>.Failed(new AuthIdentityErrorModel
                {
                    Code = ErrorCodes.Alb001Code,
                    Description = ErrorCodes.Alb001Desc
                });
            }

            var signInResult = await SignInManager.PasswordSignInAsync(identityUser, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return AuthIdentityResultModel<TokenResultModel>.Failed(new AuthIdentityErrorModel
                {
                    Code = ErrorCodes.Alb002Code,
                    Description = ErrorCodes.Alb002Desc
                });
            }
            
            var tokenResultModel = await GetToken(identityUser);

            return tokenResultModel;
        }


        public async Task<AuthIdentityResultModel<TokenResultModel>> RefreshTokenAsync(AuthLoginModel model)
        {
            // id user para obtener roles
            var identityUser = await UserManager.FindByEmailAsync(model.Email);
            if (identityUser == null)
            {
                return AuthIdentityResultModel<TokenResultModel>.Failed(new AuthIdentityErrorModel
                {
                    Code = ErrorCodes.Alb001Code,
                    Description = ErrorCodes.Alb001Desc
                });
            }
           
            var tokenResultModel = await GetToken(identityUser);

            return tokenResultModel;
        }



        #region Private methods

        /// <summary>
        /// Agregar roles al usuario, los roles deben estar previamente creados, se crearon en el SeedUserAdminAsync
        /// </summary>       
        private async Task AddRolesToUser(AuthIdentityUser authUser, bool isAdmin)
        {
            if (isAdmin)
            {
                await UserManager.AddToRoleAsync(authUser, IdentityConsts.UserRoles.Admin);
                await UserManager.AddToRoleAsync(authUser, IdentityConsts.UserRoles.User);
            }
            else
            {
                await UserManager.AddToRoleAsync(authUser, IdentityConsts.UserRoles.User);
            }
        }


        /// <summary>
        /// Define los Claims del token
        /// </summary>        
        private async Task<IEnumerable<Claim>> GetCustomClaims(AuthIdentityUser authUser, DateTimeOffset notBefore, DateTimeOffset expiration)
        {

            var authClaims = new List<Claim>
            {
                // Identifica el sujeto del token, el usuario o la entidad a la que se refiere el token,se usa para almacenar el ID de usuario o el nombre
                new Claim(JwtRegisteredClaimNames.Sub, authUser.Id),  
                new Claim(JwtRegisteredClaimNames.Email, authUser.Email),
                //Propósito: Permite al receptor del token verificar su procedencia y autenticidad.
                new Claim(JwtRegisteredClaimNames.Iss, JwtOptions.ValidIssuer), 
                new Claim(JwtRegisteredClaimNames.Aud, JwtOptions.ValidAudience), //Identifica los destinatarios previstos del token.                
                //new Claim(JwtRegisteredClaimNames.Nbf, $"{notBefore.ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Exp, $"{expiration.ToUnixTimeSeconds()}")
                //scopes claims
            };

            var userRoles = await UserManager.GetRolesAsync(authUser);

            // roles
            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            return authClaims;
        }

        
        /// <summary>
        /// Genera el JWToken
        /// </summary>        
        private async Task<AuthIdentityResultModel<TokenResultModel>> GetToken(AuthIdentityUser identityUser)
        {            
            var notBefore = DateTimeOffset.UtcNow;
            var expiration = notBefore.AddMinutes(JwtOptions.TokenValidityInMinutes);

            var authClaims = await GetCustomClaims(identityUser, notBefore, expiration);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Secret));
            
            var token = new JwtSecurityToken(
                issuer: JwtOptions.ValidIssuer,
                // TODO: tomar en cuenta que el servidor de autenticación soportara multiples audiencias
                audience: JwtOptions.ValidAudience,
                claims: authClaims,
                //notBefore: notBefore.DateTime,
                expires: expiration.DateTime,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var returnModel = AuthIdentityResultModel<TokenResultModel>.Success;
            returnModel.Data = token.MapToTokenResultModel();
            returnModel.Data.ExpiresIn = $"{JwtOptions.TokenValidityInMinutes * 60}";
            returnModel.Data.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return returnModel;
        }

        



        #endregion
    }


}
