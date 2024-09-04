using System;
using System.Collections.Generic;
using System.Text;

namespace Alb.AuthServer.Application.Contracts.Identity.Dto
{
    public class TokenDto
    {
        /// <summary>
        /// Tipo de token: Bearer
        /// </summary>
        public string TokenType { get; set; } = string.Empty;


        /// <summary>
        /// Indica la cantidad de tiempo, en segundos, durante la cual el token de acceso (access_token) es válido.
        /// </summary>
        public string ExpiresIn { get; set; } = string.Empty;


        /// <summary>
        /// Representa el momento exacto en que el token expira, en formato de tiempo Unix (epoch time), 
        /// Corresponde a una fecha y hora específica en UTC cuando el token dejará de ser válido.
        /// </summary>
        public string ExpiresOn { get; set; } = string.Empty;


        /// <summary>
        /// Indica el momento en el que el token empieza a ser válido, en formato de tiempo Unix. 
        /// Antes de este tiempo, el token no se considera válido. Corresponde a una fecha y hora específica en UTC 
        /// cuando el token empieza a ser válido.
        /// </summary>
        //public string NotBefore { get; set; } = string.Empty;


        /// <summary>
        /// Es el token JWT en sí mismo, codificado en Base64. 
        /// Contiene información sobre la autenticación del usuario y, cuando se decodifica, puede incluir afirmaciones (claims) 
        /// como la identidad del usuario, roles, permisos, etc. 
        /// Este token es el que se envía en las solicitudes HTTP para acceder a recursos protegidos.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
    }
}
