using Microsoft.EntityFrameworkCore;
namespace Financial.Api.Data;
using Models;
using System.Threading.Tasks;

public class CosmosDbContext : DbContext
{
    public CosmosDbContext(DbContextOptions<CosmosDbContext> options)
            : base(options)
    {
    }
    public DbSet<Account> Accounts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .ToContainer("Accounts")
            .HasPartitionKey(a => a.RecordCode)
            .HasKey(a => a.Id);

        base.OnModelCreating(modelBuilder);
    }
    public async Task EnsureCreatedAsync()
    {
        await Database.EnsureCreatedAsync();
    }
}
