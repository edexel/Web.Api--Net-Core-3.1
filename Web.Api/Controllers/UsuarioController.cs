using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common.Http;
using Web.Api.Common.Interfaces;
using Web.Api.Core.Log;
using Web.Api.Core.Logic.User;
using Web.Api.Data.Models;
using Web.Api.Object.FilterRequest.User;
using Web.Api.Object.Requests.User;

namespace Web.Api.Controllers
{
  
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UsuarioController : ApiBaseController, ICatalogController<UserRequest, UserFilterRequest>
    {

        private readonly IMapper _mapper;
        public UsuarioController(IMapper mapper) => _mapper = mapper;

        // GET: api/Usuario
        [HttpDelete]
        [Route("[action]")]
        public HttpResponseJson DeleteOne(int Id)
        {
            try
            {
                var vObjRes = UserLogic.Delete(Id);
                return vObjRes ? ResponseJson.HttpResponse(true, vObjRes, "Success", 0) :
                                    ResponseJson.HttpResponse(false, vObjRes, "Record not found", 0);
            }
            catch (Exception ex)
            {
                LogErrorLogic.BuildError(ex);
                return ResponseJson.HttpResponse(false, null, ex.InnerException.Message, 0);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public HttpResponseJson GetData(UserFilterRequest objRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ResponseJson.HttpResponse(false, ModelState, "");

                var resp = UserLogic.Get(objRequest);

                return resp.data.Count() > 0 ? ResponseJson.HttpResponse(true, resp.data, "Success", resp.data.Count()) :
                                         ResponseJson.HttpResponse(false, resp.data, "No se encontraron resultados.", 0);
            }
            catch (Exception ex)
            {
                LogErrorLogic.BuildError(ex);
                return ResponseJson.HttpResponse(false, null, ex.InnerException.Message, 0);
            }
        }


        [HttpPost]
        [Route("[action]")]
        public HttpResponseJson SaveData(UserRequest ObjRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ResponseJson.HttpResponse(false, ModelState, "");

                var vObjRes = UserLogic.Save(_mapper.Map<CUser>(ObjRequest));
                return vObjRes ? ResponseJson.HttpResponse(true, vObjRes, "Success", 0) :
                                    ResponseJson.HttpResponse(false, vObjRes, "Unable to save data", 0);
            }
            catch (Exception ex)
            {
                LogErrorLogic.BuildError(ex);
                return ResponseJson.HttpResponse(false, null, ex.InnerException.Message, 0);
            }
        }

    }
}
