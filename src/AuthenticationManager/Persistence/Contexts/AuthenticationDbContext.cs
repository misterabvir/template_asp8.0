using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Contexts;
public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
    : DbContext(options)
{
    private const string DefaultSchema = "authentication_schema";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthenticationDbContext).Assembly);
    }
}

public class AuthenticationDbContextDesignFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
{
    private const string DefaultConnectionString =
        "Server=127.0.0.1,1433;Database=authentication_manager_db;User Id=sa;Password=Strong_$_Password;Encrypt=false;";
    
    public AuthenticationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>()
            .UseSqlServer(DefaultConnectionString)
            .UseSnakeCaseNamingConvention();
        return new AuthenticationDbContext(optionsBuilder.Options);
    }
}