using ConsoleApp1.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Context
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("stub", b =>
                {
                    b.MigrationsAssembly(typeof(DataContext).Assembly.FullName);
                    b.UseNodaTime();
                    b.SetPostgresVersion(13, 2);
                });

            return new DataContext(optionsBuilder.Options);
        }
    }
}
