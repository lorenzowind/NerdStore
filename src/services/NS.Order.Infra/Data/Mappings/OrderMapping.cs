using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NS.Order.Infra.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Domain.Orders.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Orders.Order> builder)
        {
            builder.HasKey(ord => ord.Id);

            builder.OwnsOne(ord => ord.Address, a =>
            {
                a.Property(ad => ad.PublicArea)
                    .HasColumnName("PublicArea");

                a.Property(ad => ad.Number)
                    .HasColumnName("Number");

                a.Property(ad => ad.Complement)
                    .HasColumnName("Complement");

                a.Property(ad => ad.District)
                    .HasColumnName("District");

                a.Property(ad => ad.ZipCode)
                    .HasColumnName("ZipCode");

                a.Property(ad => ad.City)
                    .HasColumnName("City");

                a.Property(ad => ad.State)
                    .HasColumnName("State");
            });

            builder.Property(ord => ord.Code)
                .HasDefaultValueSql("NEXT VALUE FOR OrdersSequence");

            builder.HasMany(ord => ord.OrderItens)
                .WithOne(ord => ord.Order)
                .HasForeignKey(ord => ord.OrderId);

            builder.ToTable("Orders");
        }
    }
}
