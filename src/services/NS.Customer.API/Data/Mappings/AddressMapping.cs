using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NS.Customer.API.Models;

namespace NS.Customer.API.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(ad => ad.Id);

            builder.Property(ad => ad.PublicArea)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(ad => ad.Number)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(ad => ad.ZipCode)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(ad => ad.Complement)
                .HasColumnType("varchar(250)");

            builder.Property(ad => ad.District)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(ad => ad.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(ad => ad.State)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Addresses");
        }
    }
}