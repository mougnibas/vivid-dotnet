// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Aspire.Hosting;

namespace Vivid.Aspire.InDatabaseMariadb
{
    /// <summary>
    /// Entry point for the Vivid Aspire InDatabaseMariadb application.
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

            // Declare a mysql database.
            var mysql = builder.AddMySql("mysql").WithPhpMyAdmin();
            var mysqlDb = mysql.AddDatabase("KernelDb");

            // Add the kernel project, with mysql dependency (add wait for the DB to be ready).
            builder.AddProject<Projects.VividKernelWebserviceExeInDatabaseMariadb>("kernel")
                .WithReference(mysqlDb)
                .WaitFor(mysqlDb);

            // Build and run the application.
            builder.Build().Run();
        }
    }
}
