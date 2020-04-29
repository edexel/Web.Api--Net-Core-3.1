using System;
using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{

    public class ApiBaseController : ControllerBase
    {

        public static DateTime dtIni;

        public ApiBaseController()
        {
            dtIni = DateTime.Now;// Iniciar la medición.-

        }



        public static string GetTime(HttpRequestMessage url)
        {
            var dtEnd = DateTime.Now; // Finaliza la medición.-
            int ulti = (dtEnd.Millisecond < dtIni.Millisecond) ? ((dtEnd.Millisecond - dtIni.Millisecond)) * -1 : (dtEnd.Millisecond - dtIni.Millisecond);

            var time = (dtEnd.Minute - dtIni.Minute).ToString("00") + ":" + (dtEnd.Second - dtIni.Second).ToString("00") + "." + (ulti).ToString("000");

            return time;
        }

    }
}