using Alb.AuthServer.Domain.Shared.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alb.AuthServer.Application.Contracts.Identity.Dto
{
    public class LoginUserDto
    {
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

    }
}
