using DataLayer.Models;
using DataLayer.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo
{
    public interface IItemRepo : IGeneric<Item>
    {
        public IEnumerable<Item> FindAllIncludeArticles(Expression<Func<Item, bool>> expression);
    }
}
