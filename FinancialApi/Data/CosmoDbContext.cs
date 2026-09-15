using Financial.Api.Data.Configuration;
using Microsoft.EntityFrameworkCore;
namespace Financial.Api.Data;
using Financial.Shared;
using System.Threading.Tasks;

public class CosmosDbContext : DbContext
{
    public CosmosDbContext(DbContextOptions<CosmosDbContext> options)
            : base(options)
    {
    }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<FinancialEvent> Events { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new FinancialEventConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    public async Task EnsureCreatedAsync()
    {
        await Database.EnsureCreatedAsync();
    }
}
