using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CleanSample.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CleanSampleDbContext>
{
    public CleanSampleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CleanSampleDbContext>();
        optionsBuilder.UseSqlServer("Server=WALEED-LENOVO\\SQLEXPRESS;Database=CleanSampleDb;User Id=sa;Password=123456;TrustServerCertificate=true;");

        return new CleanSampleDbContext(optionsBuilder.Options);
    }
}
