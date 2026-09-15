using Financial.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financial.Api.Data.Configuration;

public class TransactionConfiguration: IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToContainer("Transactions")
            .HasPartitionKey(x => x.RecordCode)
            .HasKey(x => x.Id);
    }
}
