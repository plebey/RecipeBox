using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using RecipeBox.Data;

namespace RecipeBox.Tests.IntegrationTests
{
    internal class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? _connection;

        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<DBContextRecipeBox>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();
                services.AddDbContext<DBContextRecipeBox>(options =>
                {
                    options.UseSqlite(_connection);
                });
            });
        }

        public void InitDatabase()
        {
            using (var scope = Services.CreateScope())
            {
                var db = scope.ServiceProvider
                              .GetRequiredService<DBContextRecipeBox>();

                db.Database.EnsureCreated();
            }
        }
    }
}
