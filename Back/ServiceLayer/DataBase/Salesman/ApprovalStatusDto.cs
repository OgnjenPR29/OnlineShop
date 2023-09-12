using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.Salesman
{
    public class ApprovalStatusDto : IDTO
    {
        public bool SalesmanStatus { get; set; }
        public string SalesmanName { get; set; }
    }
}
