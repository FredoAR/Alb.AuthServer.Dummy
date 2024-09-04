

namespace Alb.AuthServer.Domain.Models.Identity
{
    public class AuthCreateUserModel
    {
        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>       
        //public string UserName { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>         
        public string Email { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>         
        public string Password { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;


        public bool? IsInternalClient { get; set; } = null;

        
        public int RegistrationSource { get; set; }
    }
}
