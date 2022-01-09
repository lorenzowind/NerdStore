using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NS.Order.Domain.Orders;

namespace NS.Order.Infra.Data.Mappings
{
    public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(ord => ord.Id);

            builder.Property(ord => ord.ProductName)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.HasOne(ord => ord.Order)
                .WithMany(ord => ord.OrderItens);

            builder.ToTable("OrderItens");
        }
    }
}
