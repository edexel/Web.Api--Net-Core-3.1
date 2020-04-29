using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Object.FilterRequest.User
{
    public class UserFilterRequest : FilterModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
    }
}
