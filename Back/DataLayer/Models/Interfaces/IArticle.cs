using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Interfaces
{
    public interface IArticle
    {
		long Id { get; set; }

		string Name { get; set; }

		string Description { get; set; }

		int Quantity { get; set; }

		double Price { get; set; }

		string Image { get; set; }

		long SalesmanId { get; set; }

		Salesman Salesman { get; set; }
		ICollection<Item> Items { get; set; }


	}
}
