using Financial.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financial.Api.Data.Configuration;

public class FinancialEventConfiguration: IEntityTypeConfiguration<FinancialEvent>
{
    public void Configure(EntityTypeBuilder<FinancialEvent> builder)
    {
        builder.ToContainer("FinancialEvents")
            .HasPartitionKey(x => x.RecordCode)
            .HasKey(x => x.Id);
    }
}
