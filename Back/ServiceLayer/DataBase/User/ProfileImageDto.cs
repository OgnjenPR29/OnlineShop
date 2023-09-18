using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.User
{
    public class ProfileImageDto : IDTO
    {
        public IFormFile ProfileImage { get; set; }

    }
}
