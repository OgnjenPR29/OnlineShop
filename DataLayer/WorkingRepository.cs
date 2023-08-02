using DataLayer.Repo;
using DataLayer.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class WorkingRepository : IWorkingRepository
    {
		private OnlineShopContext _context;

		public WorkingRepository(OnlineShopContext context)
		{
			_context = context;
			SalesmanRepository = new SalesmanRepo(context);
			AdminRepository = new AdminRepo(context);
			ShopperRepository = new ShopperRepo(context);
			ArticleRepository = new ArticleRepo(context);
			OrderRepository = new OrderRepo(context);

		}

		public ISalesmanRepo SalesmanRepository { get; set; }
		public IOrderRepo OrderRepository { get; set; }
		public IAdminRepo AdminRepository { get; set; }
		public IShopperRepo ShopperRepository { get; set; }
		public IArticleRepo ArticleRepository { get; set; }

		public void Commit()
		{
			_context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
