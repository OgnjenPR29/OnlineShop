using DataLayer.Repo;
using DataLayer.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IWorkingRepository
    {
        IAdminRepo AdminRepository { get; set; }
        ISalesmanRepo SalesmanRepository { get; set; }
        IShopperRepo ShopperRepository { get; set; }
        IArticleRepo ArticleRepository { get; set; }
        IOrderRepo OrderRepository { get; set; }
        IItemRepo ItemRepository { get; set; }
        void Commit();

    }
}
