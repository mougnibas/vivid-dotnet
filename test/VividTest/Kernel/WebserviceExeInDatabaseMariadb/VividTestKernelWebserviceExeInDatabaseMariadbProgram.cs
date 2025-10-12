// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using Vivid.Kernel.Service;
using Vivid.Kernel.WebserviceExeInDatabaseMariadb;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Vivid.Kernel.DataAccessInDatabase;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Npgsql;
using MySqlConnector;

namespace VividTest.Kernel.WebserviceExeInDatabaseMariadb
{
    [TestClass]
    [TestCategory("Integration")]
    public sealed class VividTestKernelWebserviceExeInDatabaseMariadbProgram
    {
        private static IContainer? _mariadbInstance;

        private static WebApplicationFactory<Program>? _factory;

        private static HttpClient? _client;

        [ClassInitialize]
        public static async Task SetupOnce(TestContext context)
        {
            // MariaDB fixed values for instance and datasource.
            const string mariadbHost = "localhost";
            const string mariadbPort = "3306";
            const string mariadbRootPassword = "myreallysecretpassword";
            const string mariadbUser = "kernelDbUser";
            const string mariadbPassword = "mysecretpassword";
            const string mariadbDatabase = "kernel_testcontainer";

            // Create the MariaDB container instance.
            _mariadbInstance = new ContainerBuilder()
                .WithImage("mariadb:latest")
                .WithEnvironment("MYSQL_ROOT_PASSWORD", mariadbRootPassword)
                .WithEnvironment("MYSQL_DATABASE", mariadbDatabase)
                .WithEnvironment("MYSQL_USER", mariadbUser)
                .WithEnvironment("MYSQL_PASSWORD", mariadbPassword)
                .WithPortBinding(mariadbPort, mariadbPort)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(int.Parse(mariadbPort)))
                .Build();

            // Start the MariaDB container instance.
            await _mariadbInstance.StartAsync();

            // We create a mariadb datasource because we will need it at app service configuration step.
            string connectionString = $"Server={mariadbHost};Port={mariadbPort};Database={mariadbDatabase};User={mariadbUser};Password={mariadbPassword};AllowUserVariables=True;UseAffectedRows=False;";
            MySqlDataSource mariadbDataSource = new MySqlDataSourceBuilder(connectionString).Build();

            // Create the factory and client.
            // Program actually need this datasource to be configured.
            // It's the job of Aspire orchestration, but in tests, we need to do it manually.
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(mariadbDataSource);
                });
            });
            _client = _factory.CreateClient();
        }

        [TestInitialize]
        public async Task Setup()
        {
            // Create the database.
            VividKernelDbContext dbContext = _factory!.Services.CreateScope().ServiceProvider.GetRequiredService<VividKernelDbContext>();
            dbContext?.Database.EnsureCreated();

            // Populate initial data.
            await _client!.PostAsync("/kernel/customer",
                new StringContent(
                    JsonSerializer.Serialize(new VividKernelCustomer { Id = "my-id", Secret = "my-secret" }),
                System.Text.Encoding.UTF8, "application/json"));
            await _client!.PostAsync("/kernel/customer",
                new StringContent(
                    JsonSerializer.Serialize(new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" }),
                System.Text.Encoding.UTF8, "application/json"));
        }

        [TestCleanup]
        public void TearDown()
        {
            VividKernelDbContext dbContext = _factory!.Services.CreateScope().ServiceProvider.GetRequiredService<VividKernelDbContext>();
            dbContext?.Database.EnsureDeleted();
        }

        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static async Task TearDownOnce()
        {
            _client?.Dispose();
            _client = null;

            _factory?.Dispose();
            _factory = null;

            if (_mariadbInstance != null)
            {
                await _mariadbInstance.StopAsync();
                await _mariadbInstance.DisposeAsync();
                _mariadbInstance = null;
            }
        }

        [TestMethod]
        public async Task SendPostToCustomerWithJsonShouldReturnThisNewCustomer()
        {
            // Arrange.
            VividKernelCustomer customer = new VividKernelCustomer { Id = "my-id-3", Secret = "my-secret-3" };
            string expected = JsonSerializer.Serialize(customer);

            // Act.
            HttpResponseMessage response = await _client!.PostAsync("/kernel/customer", new StringContent(expected, System.Text.Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string actual = await response.Content.ReadAsStringAsync();

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task SendPostToCustomerShouldReturnOneMoreCustomer()
        {
            // Arrange.

            // Initial count.
            HttpResponseMessage responseCustomers = await _client!.GetAsync("/kernel/customer");
            var customersArray = JsonSerializer.Deserialize<VividKernelCustomer[]>(await responseCustomers.Content.ReadAsStringAsync());
            int initialCount = customersArray != null ? customersArray.Length : 0;
            int expected = initialCount + 1;

            // Add a new one.
            VividKernelCustomer newCustomer = new VividKernelCustomer { Id = "my-id-3", Secret = "my-secret-3" };
            string newCustomerJson = JsonSerializer.Serialize(newCustomer);
            _ = await _client.PostAsync(
                "/kernel/customer",
                new StringContent(newCustomerJson, System.Text.Encoding.UTF8, "application/json")
            );

            // Act.
            // New counter after added a new one.
            HttpResponseMessage responseCustomersAfterAdded = await _client.GetAsync("/kernel/customer");
            var customersArrayAfterAdded = JsonSerializer.Deserialize<VividKernelCustomer[]>(await responseCustomersAfterAdded.Content.ReadAsStringAsync());
            int actual = customersArrayAfterAdded != null ? customersArrayAfterAdded.Length : 0;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        
        [TestMethod]
        public async Task SendGetToCustomerWithUnknownIdShouldReturn404()
        {
            // Arrange.
            int expected = new NotFoundResult().StatusCode;

            // Act.
            HttpResponseMessage response = await _client!.GetAsync("/kernel/customer/my-id-not-found");
            int? actual = (int?)response.StatusCode;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task SendGetToCustomerWithMyIdParameterShouldReturnThatCustomer()
        {

            // Arrange
            VividKernelCustomer customer = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };
            string expected = JsonSerializer.Serialize(customer);

            // Act.
            HttpResponseMessage response = await _client!.GetAsync("/kernel/customer/my-id");
            response.EnsureSuccessStatusCode();
            string actual = await response.Content.ReadAsStringAsync();

            // Assert.
            Assert.AreEqual(expected, actual);
        }
    }
}
