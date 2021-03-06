﻿using System;
using System.Collections.Generic;

namespace Web.Api.Dal.Models
{
    public partial class CUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateIni { get; set; }
        public bool? Active { get; set; }
    }
}
