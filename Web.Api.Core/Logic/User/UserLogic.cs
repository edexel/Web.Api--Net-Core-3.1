
using AutoMapper;
using NICEAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Web.Api.Data.Models;
using Web.Api.Object.FilterRequest.User;
using Web.Api.Object.Requests.Auth;
using Web.Api.Object.Responses.Auth;
using Web.Api.Common.Helpers;

namespace Web.Api.Core.Logic.User
{
    public  class UserLogic
    {

        private static  IMapper _mapper;

         public UserLogic(IMapper mapper)
        {
            _mapper = mapper;
        }



        /// <summary>
        ///  Función que envia datos y parametros para consulta filtar una busqueda
        /// </summary>
        /// <param name="objRequest"></param>
        /// <returns></returns>
        public static (IQueryable<CUser> data, int totalRows) Get(UserFilterRequest objRequest)
        {
            var unitOfWork = new UnitOfWork(new TestDBContext());
            // Forma el Query d ela consulta
            var where = GetWhere(objRequest);
            var Containers = unitOfWork.SelectPaged<CUser>(objRequest.page == 0 ? 1 : objRequest.page, objRequest.rows, x => x.Id, where);

            return Containers;
        }
        /// <summary>
        /// Función para generar cadena de filtros dependiendo los parametros enviados.
        /// </summary>
        /// <param name="objRequest"></param>
        /// <returns></returns> 
        private static System.Linq.Expressions.Expression<Func<CUser, bool>> GetWhere(UserFilterRequest objRequest)
        {
            var where = "1=1";

            if (objRequest.Id != 0)
            {
                where += "and id =" + objRequest.Id;
            }
            if (objRequest.Name != "" && objRequest.Name != null)
            {
                where += " and Name.ToUpper().Contains(\"" + objRequest.Name.ToUpper() + "\")";
            }
            if (objRequest.Email != "" && objRequest.Email != null)
            {
                where += " and Email.ToUpper().Contains(\"" + objRequest.Email.ToUpper() + "\")";
            }
            if (objRequest.Active != null)
                where += "and Active =" + objRequest.Active;


            var e = (Expression)DynamicExpressionParser.ParseLambda(new[] { Expression.Parameter(typeof(CUser), "x") }, null, where);
            return where.Length > 0 ? (Expression<Func<CUser, bool>>)e : null;

        }

        /// <summary>
        /// Función para enviar guartdar un registro
        /// </summary>
        /// <param name="ObjectRequest"></param>
        /// <returns></returns>
        public static bool Save(CUser ObjectRequest)
        {
            var unitOfWork = new UnitOfWork(new TestDBContext());

            if (ObjectRequest.Id == 0)
            {
                ObjectRequest.Active = true;
                ObjectRequest.DateIni = DateTime.Now;
                unitOfWork.Insert(ObjectRequest);
            }
            else
            {
                unitOfWork.Update(ObjectRequest);
            }

            unitOfWork.Commit();
            unitOfWork.Detach(ObjectRequest);
            unitOfWork.Dispose();

            return true;
        }


        /// <summary>
        /// Función para Eliminar registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(int id)
        {
            var unitOfWork = new UnitOfWork(new TestDBContext());
            //Consulta la compañia
            var vObject = GetById(id);

            //Elimina el registro
            vObject.Active = false;

            unitOfWork.Update(vObject);
            //Guarda el cambio
            unitOfWork.Commit();
            unitOfWork.Detach(vObject);
            unitOfWork.Dispose();

            return true;
        }

        /// <summary>
        /// Función que busca un registro por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CUser GetById(int id)
        {
            var db = new TestDBContext();

            return db.CUser.Where(x => x.Id == id).FirstOrDefault();
        }

        public static (AuthResponse data, bool status, string msj) CheckUserLogin(AuthRequest oAuth)
        {
            var db = new TestDBContext();
            var response = new AuthResponse();
            var oUser = db.CUser.Where(x => x.Name == oAuth.Name && x.Password == oAuth.Password).FirstOrDefault();

            if (oUser == null)
                return (null, false, "wrong username or password");


            // datos con los que se crearan el token
            Dictionary<string, string> userInfo = new Dictionary<string, string>
            {
                {  "Name" , oUser.Password },
                {  "Id" , oUser.Id.ToString() }

            };
            // Genera token
            //response.Token = JwtManager.GenerateToken(userInfo);
            response.Token = JwtManagerCore.GenerateTokenCore(oUser);
            response.Name = oUser.Name;
            response.Id = oUser.Id;
            response.Email = oUser.Email;

            //return (response,resp);
            return (response, true,"Success");
        }
    }

}
