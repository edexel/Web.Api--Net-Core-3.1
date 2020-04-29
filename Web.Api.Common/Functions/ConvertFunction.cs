using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NICEAPI.Common.Functions
{
   public static class ConvertFunction
    {

        /// <summary>
        /// Convierte lista de string en formato json a un array de datos
        /// </summary>
        /// <param name="listResult"></param>
        /// <returns></returns>
        public static JArray ConvertJsonListToString(List<string> listResult)
        {
            string result = "";

            foreach (string obj in listResult)
            {
                result += obj;
            }

            return JArray.Parse(result);

        }

        /// <summary>
        /// Convierte lista de string en formato json a un array de datos
        /// </summary>
        /// <param name="listResult"></param>
        /// <returns></returns>
        public static string ConvertListToString(List<string> listResult)
        {
            string result = "";

            foreach (string obj in listResult)
            {
                result += obj;
            }

            return result;

        }


    }
}
