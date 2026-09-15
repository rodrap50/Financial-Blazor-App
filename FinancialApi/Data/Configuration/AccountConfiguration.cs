using Financial.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financial.Api.Data.Configuration;

public class AccountConfiguration: IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder
            .ToContainer("Accounts")
            .HasPartitionKey(a => a.RecordCode)
            .HasKey(a => a.Id);
    }
}
