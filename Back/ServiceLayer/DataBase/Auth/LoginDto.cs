using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.Auth
{
    public class LoginDto : IDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
