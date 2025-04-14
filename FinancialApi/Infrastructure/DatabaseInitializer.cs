using Financial.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Api.Infrastructure;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly CosmosDbContext _context;

    public DatabaseInitializer(CosmosDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync() =>
        // This creates the database and containers if they don't exist
        await _context.Database.EnsureCreatedAsync();
}
