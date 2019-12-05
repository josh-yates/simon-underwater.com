using System;
using System.IO;
using System.Reflection;
using Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Utilities
{
    public static class StartupExtensions
    {
        public static void AddAppDbContext(this IServiceCollection services, IConfigurationSection databaseSection)
        {
            if (databaseSection == default(IConfigurationSection))
            {
                throw new ArgumentNullException(nameof(databaseSection));
            }

            var databaseType = databaseSection.GetValue<string>("Type") ??
                throw new InvalidDataException("Database configuration does not contain 'Type'");

            services.AddDbContextPool<AppDbContext>(options => 
            {
                switch (databaseType.ToUpper())
                {
                    case "SQLSERVER":
                        var connectionString = databaseSection.GetValue<string>("ConnectionString") ??
                            throw new InvalidDataException("Database connection string not found");
                        options.UseSqlServer(connectionString, s => s.MigrationsAssembly("Migrations"));
                        break;
                    case "INMEMORY":
                        var databaseName = databaseSection.GetValue<string>("Name") ??
                            throw new InvalidDataException("Database name not found");
                        options.UseInMemoryDatabase(databaseName);
                        break;
                    default:
                        throw new InvalidDataException("Database type not recognised");
                }
            });
        }

        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app
                .ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<AppDbContext>())
                {
                    // TODO see if this can be async
                    context.Database.Migrate();
                }
            }
        }
    }
}