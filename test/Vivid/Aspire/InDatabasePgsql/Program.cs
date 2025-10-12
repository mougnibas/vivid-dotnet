// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Aspire.Hosting;

namespace Vivid.Aspire.InDatabasePgsql
{
    /// <summary>
    /// Entry point for the Vivid Aspire InDatabasePgsql application.
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

            // Declare a postgresql database.
            var pgsql = builder.AddPostgres("pgsql").WithImage("postgres:18.0-bookworm")
                        .WithPgAdmin(pgAdmin => pgAdmin.WithImage("dpage/pgadmin4:9.8.0"));
            var pgsqlDb = pgsql.AddDatabase("KernelDb");

            // Add the kernel project, with pgsql dependency (add wait for the DB to be ready).
            builder.AddProject<Projects.VividKernelWebserviceExeInDatabasePgsql>("kernel")
                .WithReference(pgsqlDb)
                .WaitFor(pgsqlDb);

            // Build and run the application.
            builder.Build().Run();
        }
    }
}
