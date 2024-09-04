using Alb.AuthServer.Application.Contracts.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Alb.AuthServer.Application.Contracts.Identity.Dto
{
    public class TokenResultDto
    {
        private static readonly TokenResultDto _success = new TokenResultDto { Succeeded = true, DateUtc = DateTimeOffset.UtcNow };
        // priotected para que pueda ser visible desde la clase generica que la va a heredar
        protected readonly List<AuthError> _errors = new List<AuthError>();


        public static TokenResultDto Success => _success;


        [JsonProperty("succeeded")]
        public bool Succeeded { get; protected set; }
        

        [JsonProperty("errors")]
        public IEnumerable<AuthError> Errors => _errors;


        [JsonProperty("dateUtc")]
        public DateTimeOffset DateUtc { get; set; }


        [JsonProperty("token")]
        public TokenDto Token { get; set; }




        public static TokenResultDto Failed(params AuthError[] errors)
        {
            var result = new TokenResultDto { Succeeded = false, DateUtc = DateTimeOffset.UtcNow };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }


    }
    
    
}
