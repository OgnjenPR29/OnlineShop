using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.Auth
{
    public class JwtDto : IDTO
    {
        public JwtDto(string token)
        {
            Token = token;
        }

        public string Token { get; set; }

    }
}
