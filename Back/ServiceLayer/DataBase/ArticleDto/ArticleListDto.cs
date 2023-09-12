using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.ArticleDto
{
    public class ArticleListDto : IDTO
    {
       public List<ArticleDetailDto> Articles { get; set; }
    }
}
