using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.Salesman
{
    public class SalesmanListDto : IDTO
    {
        public List<SalesmanDto> Salesmans { get; set; }
    }
}
