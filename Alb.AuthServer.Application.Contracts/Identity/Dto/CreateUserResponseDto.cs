using Alb.AuthServer.Application.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alb.AuthServer.Application.Contracts.Identity.Dto
{
    public class CreateUserResponseDto
    {
        private static readonly CreateUserResponseDto _success = new CreateUserResponseDto { Succeeded = true, DateUtc = DateTimeOffset.UtcNow };
        private readonly List<AuthError> _errors = new List<AuthError>();



        public bool Succeeded { get; protected set; }


        public static CreateUserResponseDto Success => _success;
            
        
        public IEnumerable<AuthError> Errors => _errors;


        public DateTimeOffset DateUtc { get; set; }


        public static CreateUserResponseDto Failed(params AuthError[] errors)
        {
            var result = new CreateUserResponseDto { Succeeded = false, DateUtc = DateTimeOffset.UtcNow };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

    }


   
}
