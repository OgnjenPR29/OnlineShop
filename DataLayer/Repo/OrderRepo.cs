using DataLayer.Models;
using DataLayer.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo
{
    public class OrderRepo : Generic<Order>, IOrderRepo
    {
        private readonly object dataBaseLockControl = new object();

        public OrderRepo(OnlineShopContext context) : base(context)
        {
        }

        public IEnumerable<Order> ItemsAndArticles(Expression<Func<Order, bool>> expression)
        {
            lock (dataBaseLockControl)
            {
                var result = _context.Set<Order>().Include(order => order.Items).ThenInclude(item => item.Article).Where(expression).ToList();

                return result;
            }
        }
    }
}
