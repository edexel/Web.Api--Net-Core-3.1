using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Data.Models;

namespace Web.Api.Common.Helpers
{
    public class JwtManagerCore
    {

        public static string GenerateTokenCore(CUser oUser, int expireMinutes = 1440)
        {
            var claims = new[]
            {
                 new Claim("UserTokenData",JsonConvert.SerializeObject(oUser))
             };



            var token = new JwtSecurityToken(
                audience: "API_Rest",
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                notBefore: DateTime.Now,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.GetjwtSecret())), SecurityAlgorithms.HmacSha256)

                );
            return   new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
