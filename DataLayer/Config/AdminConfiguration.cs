using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Config
{
    internal class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
		public void Configure(EntityTypeBuilder<Admin> builder)
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

			builder.Property(x => x.Image).HasColumnType("varchar(100)");
		}
	}
}
