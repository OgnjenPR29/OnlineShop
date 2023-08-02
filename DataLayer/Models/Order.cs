using DataLayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Order : IOrder
    {
		public long Id { get; set; }

		public double TotalPrice { get; set; }

		public DateTime Created { get; set; }

		public int DeliveryInSeconds { get; set; }

		public string Comment { get; set; }

		public string Address { get; set; }

		public long? ShopperId { get; set; }

		public Shopper Shopper { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
