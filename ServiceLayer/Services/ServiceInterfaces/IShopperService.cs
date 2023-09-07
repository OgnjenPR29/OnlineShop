using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.ServiceInterfaces
{
    public interface IShopperService
    {
        public IServiceOperationResult GetAllArticles();
    }
}
