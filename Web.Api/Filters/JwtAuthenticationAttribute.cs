using Web.Api.Common.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Web.Api.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;
        public static Int32 CompanyID = 0;
        public static Int32 UserID = 0;
        //public static string _plataforma = "OrderEntry";


        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {
                var request = context.Request;
                var authorization = request.Headers.Authorization;

                if (authorization == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                    return;
                }

                var token = authorization.ToString();
                var principal = await AuthenticateJwtToken(token);

                if (principal == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
                }
                else
                {
                    //var url = context.Request.RequestUri.AbsoluteUri;
                    //LogRequestLogic.LogApi(_plataforma, url, UserID, "");
                    context.Principal = principal;
                }
            }
            catch (Exception )
            {

                throw;
            }
       
        }

        private static bool ValidateToken(string token, out string username)
        {
            username = null;

            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            //var companyClaim = identity.FindFirst(ClaimTypes.GroupSid);

            //var Company = companyClaim?.Value;

            //CompanyID = Convert.ToInt32(Company);

            var userIDClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var Id = userIDClaim?.Value;

            UserID = Convert.ToInt32(Id);

           

            if (string.IsNullOrEmpty(username))
                return false;

            // More validate to check whether username exists in system

            return true;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            string username;
                  
            if (ValidateToken(token, out username))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {

            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}
