using System;
using Microsoft.Extensions.Configuration;

namespace ClinicSystem.DAL.Global
{
    public static class DataAccessSetting
    {
        // Static property to hold the connection string
        public static string ConnectionString { get; private set; } = string.Empty;

        // Method to initialize from IConfiguration
        public static void Initialize(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            ConnectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string not found in configuration");
        }
    }
}
