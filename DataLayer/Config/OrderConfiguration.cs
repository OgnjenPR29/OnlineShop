using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Config
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder.HasMany(x => x.Items)
				.WithOne(x => x.Order)
				.HasForeignKey(x => x.OrderId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
