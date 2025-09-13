// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using Aspire.Hosting;

namespace Vivid.Aspire.InDatabaseInMemory
{
    /// <summary>
    /// Entry point for the Vivid Aspire InDatabaseInMemory application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            // Create the builder.
            IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

            // Add the kernel project, without any dependencies.
            builder.AddProject<Projects.VividKernelWebserviceExeInDatabaseInMemory>("kernel");

            // Build and run the application.
            builder.Build().Run();
        }
    }
}
