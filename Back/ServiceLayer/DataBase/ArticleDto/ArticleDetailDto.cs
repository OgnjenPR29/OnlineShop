using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.ArticleDto
{
    public class ArticleDetailDto : IDTO
    {
		public ArticleDetailDto(long articleId, string name, string description, int quantity, double price, byte[] productImage)
		{
			Id = articleId;
			Name = name;
			Description = description;
			Quantity = quantity;
			Price = price;
			Image = productImage;
		}

		public long Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }

		public byte[] Image { get; set; }
	}
}
