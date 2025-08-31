// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

using Vivid.Kernel.DataAccess;
using Vivid.Kernel.DataAccessInDatabase;
using Vivid.Kernel.Service;
using Vivid.Kernel.ServiceCore;

namespace Vivid.Kernel.WebserviceExeInDatabasePgsql
{
    /// <summary>
    /// /// The entry point for the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Create a web application builder.
            var builder = WebApplication.CreateSlimBuilder(args);

            // Add controllers support, with JSON options.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            // Before adding a DB context, we need to declare an appropriate datasource.
            // We are assuming .NET Aspire will be used as orchestration, so we are just
            // declaring it accordingly.
            // Of course, .NET Aspire need to declare the pgsql database on his own.
            // If .NET Aspire is not used, this instruction do nothing.
            builder.AddNpgsqlDataSource("KernelDb");

            // Add database context.
            // Even if the datasource is already declared, we need to declared it as option  with the DbContext.
            builder.Services.AddDbContext<VividKernelDbContext>((serviceProvider, options) =>
            {
                // Get the PostgreSQL data source (previously injected).
                NpgsqlDataSource dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();

                // We explicitely specify the data source for the DbContext.
                options.UseNpgsql(dataSource);
            });

            // Add application services.
            builder.Services.AddTransient<IVividKernelDataAccessService, VividKernelDataAccessServiceInDatabase>();
            builder.Services.AddTransient<IVividKernelService, VividKernelServiceCore>();

            // Build the application.
            var app = builder.Build();

            // Create the database (if it doesn't exist yet).
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<VividKernelDbContext>();
                db.Database.EnsureCreated();
            }

            // Map controllers.
            app.MapControllers();

            // Run the application.
            app.Run();
        }
    }
}