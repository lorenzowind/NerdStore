using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NS.Purchase.API.Models;

namespace NS.Purchase.API.Data.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Purchase)
                .WithMany(c => c.Transactions);

            builder.ToTable("Transactions");
        }
    }
}