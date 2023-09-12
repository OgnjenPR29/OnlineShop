using DataLayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Item : IItem
    {
		public long Id { get; set; }

		public double PricePerUnit { get; set; }

		public int Quantity { get; set; }

		public string ArticleName { get; set; }

		public long OrderId { get; set; }

		public Order Order { get; set; }

		public long? ArticleId { get; set; }

		public Article Article { get; set; }

	}
}
