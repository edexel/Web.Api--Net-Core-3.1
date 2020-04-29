using System;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common.Http;
using Web.Api.Core.Log;
using Web.Api.Core.Logic.User;
using Web.Api.Object.Requests.Auth;
using Web.Api.Object.Requests.User;

namespace Web.Api.Controllers
{

    [Route("api/v1/[controller]")]
    public class AuthController : ApiBaseController
    {
        [Route("Auth")]
        [HttpPost]
        public HttpResponseJson Post(AuthRequest oUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ResponseJson.HttpResponse(false, ModelState, "");

                var user = UserLogic.CheckUserLogin(oUser);

                return user.status ? ResponseJson.HttpResponse(user.status, user.data, user.msj, 0) :
                                         ResponseJson.HttpResponse(user.status, user.data, user.msj, 0);
            }
            catch (Exception ex)
            {
                LogErrorLogic.BuildError(ex);
                return ResponseJson.HttpResponse(false, null, ex.InnerException.Message, 0);
            }
        }
    }
}
