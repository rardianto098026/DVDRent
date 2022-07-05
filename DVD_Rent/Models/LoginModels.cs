using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVD_Rent.Models
{
    public class LoginModels
    {
        public string email { get; set; }
        public string password { get; set; }
        public int role { get; set; }
        public string status { get; set; }
    }
}