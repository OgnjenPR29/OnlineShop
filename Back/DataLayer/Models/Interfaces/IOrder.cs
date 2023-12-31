﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Interfaces
{
    public interface IOrder
    {
		long Id { get; set; }

		double TotalPrice { get; set; }

		DateTime Created { get; set; }

		int DeliveryInSeconds { get; set; }

		string Comment { get; set; }

		string Address { get; set; }

		long? ShopperId { get; set; }

		Shopper Shopper { get; set; }

		ICollection<Item> Items { get; set; }

	}
}
