using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo.Interfaces
{
    public interface IOrderRepo : IGeneric<Order>
    {
        public IEnumerable<Order> ItemsAndArticles(Expression<Func<Order, bool>> expression);
        public IEnumerable<Order> FindAllIncludeItems(Expression<Func<Order, bool>> expression);
        public Order FindFirstIncludeItems(Expression<Func<Order, bool>> expression);

    }
}
