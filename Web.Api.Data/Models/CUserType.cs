using System;
using System.Collections.Generic;

namespace Web.Api.Data.Models
{
    public partial class CUserType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime DateIni { get; set; }
    }
}
