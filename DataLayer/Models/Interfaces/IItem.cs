using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Interfaces
{
    public interface IItem
    {
		long Id { get; set; }

		double PricePerUnit { get; set; }

		int Quantity { get; set; }

		string ArticleName { get; set; }

		long OrderId { get; set; }

		Order Order { get; set; }

		long? ArticleId { get; set; }

		Article Article { get; set; }
	}
}
