
using Alb.AuthServer.Domain.Interfaces.Identity;
using Alb.AuthServer.Domain.Shared.Enums;
using Microsoft.AspNetCore.Identity;


namespace Alb.AuthServer.Infrastructure.EntityFrameworkCore.Entities
{
    /// <summary>
    /// Clase personalizada para extender propiedades de IdentityUser
    /// </summary>
    public class AuthIdentityUser : IdentityUser 
    {
        /// <summary>
        /// Identificador de aplicación desde la que se origina el registro del usuario
        /// </summary>
        public RegistrationSources RegistrationSource { get; set; }

        /// <summary>
        /// Nombre de aplicación desde la que se origina el registro del usuario
        /// </summary>
        public string RegistrationSourceDisplay { get; set; } = string.Empty;

        /// <summary>
        /// Indica si es un usuario/cliente de uso interno (aplicaciones de uso interno).
        /// </summary>
        public bool? IsInternalClient { get; set; } = null;
    }
}
