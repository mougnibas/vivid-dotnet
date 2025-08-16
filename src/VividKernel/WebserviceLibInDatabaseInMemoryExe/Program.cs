using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Vivid.Kernel.DataAccessService;
using Vivid.Kernel.DataAccessService.InDatabase;
using Vivid.Kernel.Service;
using Vivid.Kernel.Service.Core;
using Vivid.Kernel.WebserviceLib;

namespace Vivid.Kernel.WebserviceLib.InDatabase.InMemory.Exe;

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

        // Add controllers support.
        builder.Services.AddControllers();

        // Add database context.
        builder.Services.AddDbContext<VividDbContext>(options =>
            options.UseInMemoryDatabase("KernelDb")
        );

        // Add application services.
        builder.Services.AddTransient<IDataAccessService, DataAccessServiceInDatabase>();
        builder.Services.AddTransient<IKernelService, KernelServiceCore>();

        // Build the application.
        var app = builder.Build();

        // Map controllers.
        app.MapControllers();

        // Run the application.
        app.Run();
    }
}