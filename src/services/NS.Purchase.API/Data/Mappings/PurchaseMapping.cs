using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NS.Purchase.API.Data.Mappings
{
    public class PurchaseMapping : IEntityTypeConfiguration<Models.Purchase>
    {
        public void Configure(EntityTypeBuilder<Models.Purchase> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Ignore(c => c.CreditCard);

            builder.HasMany(c => c.Transactions)
                .WithOne(c => c.Purchase)
                .HasForeignKey(c => c.PurchaseId);

            builder.ToTable("Purchases");
        }
    }
}