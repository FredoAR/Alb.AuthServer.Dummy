namespace Alb.AuthServer.Domain.Models.Options
{
    public class JwtOptions
    {
        /// <summary>
        /// Es el destinatario o audiencia del token. 
        /// Representa las aplicaciones o usuarios que pueden recibir y usar el token.
        /// http://localhost:4200 es la dirección del cliente que espera recibir y utilizar el token.
        /// Durante la validación del token, el servidor comprobará si el valor de aud (audience) en 
        /// el token coincide con este valor. Si no coincide, la validación fallará y se rechazará el acceso.
        /// </summary>
        public string ValidAudience { get; set; } = string.Empty;


        /// <summary>
        /// Especifica quién emitió el token, es decir, la entidad que lo generó y firmó. 
        /// Este valor debe coincidir con el emisor (iss) en el token JWT.
        /// Ejemplo: http://localhost:5000 es la URL de tu servidor de autenticación, que emite los tokens JWT.
        /// Durante la validación, el servidor verifica que el valor de iss (issuer) en el token coincida con 
        /// este valor configurado. Esto asegura que el token fue emitido por una fuente de confianza.
        /// </summary>
        public string ValidIssuer { get; set; } = string.Empty;


        /// <summary>
        /// Es el secreto utilizado para firmar y validar los tokens JWT. Es una clave privada que el servidor 
        /// utiliza para asegurar la integridad y autenticidad del token.
        /// Es usada para generar la firma HMAC-SHA256 (u otro algoritmo) que asegura que el contenido del 
        /// token no haya sido alterado.
        /// </summary>
        public string Secret { get; set; } = string.Empty;


        /// <summary>
        /// Indica el tiempo durante el cual el token es válido, es decir, el período de vida del token 
        /// antes de que expire.
        /// </summary>
        public int TokenValidityInMinutes { get; set; }
        
    }
}
