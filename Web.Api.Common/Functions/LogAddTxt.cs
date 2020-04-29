using System;
using System.IO;
using System.Web;

namespace NICEAPI.Common.Functions
{
    public class LogAddTxt
    {
        // para crear el archivo
        public static void GenerarTXT(string json)
        {
            //string rutaCompleta = WebConfigurationManager.AppSettings["LogApiUrl"].ToString();
            //using (StreamWriter mylogs = File.AppendText(rutaCompleta)) //se crea el archivo si ya existe lo sobre escribe
            //{
            //    mylogs.WriteLine(json); // escribe en el documento
            //    mylogs.Close();
            //}
        }
        
    }
}
