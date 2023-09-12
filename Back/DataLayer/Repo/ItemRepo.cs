using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo
{
    public class ItemRepo : Generic<Item>, IItemRepo
    {
        private readonly object balanceLock = new object();

        public ItemRepo(OnlineShopContext context) : base(context)
        {
        }

        public IEnumerable<Item> FindAllIncludeArticles(Expression<Func<Item, bool>> expression)
        {
            lock (balanceLock)
            {
                var result = _context.Set<Item>().Include(item => item.Article).Where(expression).ToList();

                return result;
            }
        }
    }
}
