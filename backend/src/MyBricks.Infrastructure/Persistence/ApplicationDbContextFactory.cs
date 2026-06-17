using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyBricks.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Hardcode connection string and server version for design-time only
        var connectionString = "Server=localhost;Port=3306;Database=mybricks;Uid=root;Pwd=dev;";
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

        optionsBuilder.UseMySql(connectionString, serverVersion,
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
