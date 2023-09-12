using DataLayer.Models;
using ServiceLayer.DataBase.Salesman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.ServiceInterfaces
{
    public interface IAdminService
    {
        public IServiceOperationResult AllSalesmans();
        public IServiceOperationResult AllOrders();

        public IServiceOperationResult GetOrderDetails(long id);
        public IServiceOperationResult ChangeSalesmanStatus(ApprovalStatusDto status);
    }
}
