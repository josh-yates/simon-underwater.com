using System;
using Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Web.Services.Data
{
    public class AppDbContextFactory : IAppDbContextFactory
    {
        private readonly DatabaseType _databaseType;
        private readonly string _connectionString;

        public AppDbContextFactory(IConfiguration configuration)
        {
            var databaseConfig = configuration.GetSection("Database");
            var databaseType = databaseConfig.GetValue<string>("Type");

            _databaseType = databaseType == "SqlServer" ? 
                DatabaseType.SqlServer :
                throw new InvalidOperationException($"Database type {databaseType} is not supported.");

            _connectionString = databaseConfig.GetValue<string>("ConnectionString");
        }
        public IAppDbContext Create()
        {
            var dbConnection = new SqlConnection(_connectionString);
            var options = 
            return new AppDbContext();
        }

        private enum DatabaseType
        {
            SqlServer = 0
        }
    }
}