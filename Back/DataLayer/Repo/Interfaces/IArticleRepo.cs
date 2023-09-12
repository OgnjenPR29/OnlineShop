using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo.Interfaces
{
    public interface IArticleRepo : IGeneric<Article>
    {
        List<Article> GetAllArticlesFromSeller(long id);

    }
}
