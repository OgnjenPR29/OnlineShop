using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class OnlineShopContext : DbContext
    {
		public OnlineShopContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Admin> Admins { get; set; }

		public DbSet<Shopper> Shoppers { get; set; }

		public DbSet<Salesman> Salesman { get; set; }

		public DbSet<Article> Articles { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<Item> Items { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineShopContext).Assembly);
		}
	}
}
