using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.ArticleDto
{
    public class UpdateArticleImageDto
    {

			public string Name { get; set; }

			public IFormFile Image { get; set; }
		}
	}

