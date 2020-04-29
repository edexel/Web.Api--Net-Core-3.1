using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Object.Responses.Auth
{
    public class AuthResponse
    {
        public String Token { get; set; }
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }


    }
}
