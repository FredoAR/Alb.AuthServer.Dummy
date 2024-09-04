using System;
using System.Collections.Generic;
using System.Text;

namespace Alb.AuthServer.Application.Contracts.Common
{
    public class AuthError
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }
}
