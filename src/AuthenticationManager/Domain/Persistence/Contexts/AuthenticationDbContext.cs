using Domain.UserAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Domain.Persistence.Contexts;
public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
    : DbContext(options)
{
    private const string DefaultSchema = "authentication_schema";

    public DbSet<User> Users { get; set; }
    public DbSet<Outbox.Message> OutboxMessages { get; set; }

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
        "Host=localhost;Port=5432;Database=authentication_db;Username=username;Password=password";
    
    public AuthenticationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>()
            .UseNpgsql(DefaultConnectionString)
            .UseSnakeCaseNamingConvention();
        return new AuthenticationDbContext(optionsBuilder.Options);
    }
}