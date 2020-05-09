using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
namespace Healzhub.Lookup.Data.Context
{
    public class EVisionDbContextFactory : IDesignTimeDbContextFactory<EVisionDbContext>
    {
        public EVisionDbContext CreateDbContext(string[] args)
        {
            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath("../Application.Web"))
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
                .AddEnvironmentVariables()
                .Build();


            var optionsBuilder = new DbContextOptionsBuilder<EVisionDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString,
                b =>
                {
                    b.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                });
            return new EVisionDbContext(optionsBuilder.Options);
        }
    }
}
