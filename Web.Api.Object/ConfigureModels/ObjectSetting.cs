using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Object.ConfigureModels
{
    public class ObjectSetting
    {
        public string conectionStringSqlServer { get; set; }
        public string jwtSecret { get; set; }
        public string originRequest { get; set; }
    }
}
