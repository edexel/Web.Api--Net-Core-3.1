using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Web.Api.Data.Models;

namespace Web.Api.Common.Security.Authorization
{
    public static class JwtManager
    {

       

        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static string GenerateToken(Dictionary<string,string> user, int expireMinutes = 1440)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]{
                            new Claim(ClaimTypes.Name, user["Name"]),
                            new Claim (ClaimTypes.NameIdentifier,user["Id"]),
                        }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }

        public static string GetCompany(string token)
        {
            try
            {
                ClaimsPrincipal principal = GetPrincipal(token);
                string CompanyID ="";
                if (principal != null) {

                    var identity = principal?.Identity as ClaimsIdentity;

                    var companyClaim = identity.FindFirst(ClaimTypes.GroupSid);


                    CompanyID = companyClaim?.Value;
                }
                

                return CompanyID;
            }
            catch (Exception)
            {
                return "";
            }
        }


        //public static string GenerateTokenCore(CUser oUser, int expireMinutes = 1440)
        //{
        //    var claims = new[]
        //    {
        //         new Claim("UserTokenData",JsonConvert.SerializeObject(oUser))
        //     };



        //    var token  = new JwtSecurityToken(
        //        audience : "API_Rest", 
        //        claims : claims,
        //        expires : DateTime.Now.AddMinutes(expireMinutes),
        //        notBefore : DateTime.Now, 
        //        signingCredentials : new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.GetjwtSecret())))

        //}
    
    }
}