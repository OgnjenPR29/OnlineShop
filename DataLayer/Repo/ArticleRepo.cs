using DataLayer.Models;
using DataLayer.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo
{
    public class ArticleRepo : Generic<Article>, IArticleRepo
    {
        public ArticleRepo(OnlineShopContext context) : base(context)
        {
        }

        public List<Article> GetAllArticlesFromSeller(long id)
        {
            List<Article> articles = _context.Articles.Where(x => x.SalesmanId == id).ToList();

            return articles;
        }
    }
}
