using Alb.AuthServer.Application.Contracts.Common;
using Alb.AuthServer.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alb.AuthServer.Application.Contracts
{
    public static class AuthErrorCodes
    {
        public static AuthError Alb001 => new AuthError { Code = ErrorCodes.Alb001Code, Description = ErrorCodes.Alb001Desc };


    }
}
