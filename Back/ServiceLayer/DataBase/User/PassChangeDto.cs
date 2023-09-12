using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.User
{
    public class PassChangeDto : IDTO
    {
        public string NewPass { get; set; }
        public string OldPass { get; set; }
    }
}
