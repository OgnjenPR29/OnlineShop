using ServiceLayer.DataBase.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.ArticleDto
{
    public class OrderInfoDto : IDTO
    {
		public long Id { get; set; }

		public string Comment { get; set; }

		public string Address { get; set; }

		public double TotalPrice { get; set; }

		public DateTime Created { get; set; }

		public string RemainingTime { get; set; }

		public ICollection<ItemDto> Items { get; set; }
	}
}
