using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Alb.AuthServer.Domain.Shared.Validations;


namespace Alb.AuthServer.Application.Contracts.Identity.Dto
{
    public class CreateUserDto
    {
        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        //[Required]        
        //[StringLength(256)]
        //public string UserName { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>  
        [Required]
        [StringLength(64)]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = DtoErrorCodes.FormatErrorMessage)]
        public string Email { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>  
        [Required]
        [StringLength(64)]        
        public string Password { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>  
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "{0} no tiene formato valido")]
        public string PhoneNumber { get; set; } = string.Empty;


        public bool? IsInternalClient { get; set; } = null;


        [Required]
        public int RegistrationSource { get; set; }




    }
}
