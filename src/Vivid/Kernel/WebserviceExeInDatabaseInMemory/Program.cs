// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vivid.Kernel.DataAccess;
using Vivid.Kernel.DataAccessInDatabase;
using Vivid.Kernel.Service;
using Vivid.Kernel.ServiceCore;

namespace Vivid.Kernel.WebserviceExeInDatabaseInMemory
{
    /// <summary>
    /// The entry point for the application.
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

            // Add database context.
            builder.Services.AddDbContext<VividKernelDbContext>(options =>
                options.UseInMemoryDatabase("KernelDb")
            );

            // Add application services.
            builder.Services.AddTransient<IVividKernelDataAccessService, VividKernelDataAccessServiceInDatabase>();
            builder.Services.AddTransient<IVividKernelService, VividKernelServiceCore>();

            // Build the application.
            var app = builder.Build();

            // Map controllers.
            app.MapControllers();

            // Run the application.
            app.Run();
        }
    }
}
