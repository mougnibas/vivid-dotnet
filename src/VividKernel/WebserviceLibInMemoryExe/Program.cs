// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Vivid.Kernel.DataAccessService;
using Vivid.Kernel.DataAccessService.InMemory;
using Vivid.Kernel.Service;
using Vivid.Kernel.Service.Core;
using Vivid.Kernel.WebserviceLib;

namespace Vivid.Kernel.WebserviceLib.InMemory.Exe;

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

        // Add application services.
        builder.Services.AddSingleton<IDataAccessService, DataAccessServiceInMemory>();
        builder.Services.AddSingleton<IKernelService, KernelServiceCore>();

        // Build the application.
        var app = builder.Build();

        // Map controllers.
        app.MapControllers();

        // Run the application.
        app.Run();
    }
}