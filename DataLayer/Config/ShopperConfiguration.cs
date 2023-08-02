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
    internal class ShopperConfiguration : IEntityTypeConfiguration<Shopper>
	{
		public void Configure(EntityTypeBuilder<Shopper> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			builder.HasIndex(x => x.Username)
				.IsUnique();
			builder.Property(x => x.Username)
				.IsRequired();

			builder.Property(x => x.Email)
				.IsRequired();

			builder.Property(x => x.Image)
				.HasColumnType("varchar(100)");

			builder.HasMany(x => x.Orders)
				.WithOne(x => x.Shopper)
				.HasForeignKey(x => x.ShopperId)
				.OnDelete(DeleteBehavior.ClientSetNull);
		}
	}
}
