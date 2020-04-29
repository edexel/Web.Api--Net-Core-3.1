using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Api.Object.Requests.Auth
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "El {0} es requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
