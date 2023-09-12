using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.Item
{
    public class ItemDto
	{
		public long? ArticleId { get; set; }

		public double PricePerUnit { get; set; }

		public int Quantity { get; set; }

		public string ArticleName { get; set; }

		public byte[] ArticleImage { get; set; }
	}
}
