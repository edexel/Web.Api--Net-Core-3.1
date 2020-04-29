using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Web.Api.Common.Functions
{
    public class Functions
    {
        public string RemoveDiacritics(string s)
        {
            string normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static string getTable(string model)
        {
            model =  model.Replace("Request", "");
            model = model.Replace("Api", "");

            return model;
        }

        /// <summary>
        /// Agrega el numero de error a los arrores arrojados por el sistema
        /// </summary>
        /// <param name="systemError"></param>
        /// <returns></returns>
        public static string ErrorExection(string systemError)
        {
            var msj = systemError;

            switch (systemError)
            {
                case "Execution Timeout Expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.":
                    msj = "";
                    break;
                case "An error occurred while executing the command definition. See the inner exception for details.":
                    msj = "";
                    break;
                case "Object reference not set to an instance of an object.":
                    msj = "";
                    break;
                case "The underlying provider failed on Open.":
                    msj = "";
                    break;
                case "Login failed for user 'api'. Reason: The account is disabled.":
                    msj = "";
                    break;
                case "Failed to update database \"Arielito\" because the database is read-only.":
                    msj = "";
                    break;
                case "Error relacionado con la red o específico de la instancia mientras se establecía una conexión con el servidor SQL Server. No se encontró el servidor o éste no estaba accesible. Compruebe que el nombre de la instancia es correcto y que SQL Server está configurado para admitir conexiones remotas. (provider: TCP Provider, error: 0 - Tiempo de espera de la operación de espera agotado.)":
                    msj = "";
                    break;
            }

            return msj;
        }


        /// <summary>
        /// Valida si una variable es numerica
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static bool IsNumeric(string valor)
        {
            bool isNumber;
            double isItNumeric;
            isNumber = Double.TryParse(Convert.ToString(valor), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out isItNumeric);
            return isNumber;
        }
    }
}
