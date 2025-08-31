// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Aspire.Hosting;
using System;
using System.Threading.Tasks;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Net.Http;
using Vivid.Kernel.Service;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace VividTest.Aspire.InMemory
{
    [TestClass]
    public sealed class VividTestAspireInMemory
    {

        private static DistributedApplication? _app;

        [ClassInitialize]
        public static async Task SetUp(TestContext testContext)
        {
            IDistributedApplicationTestingBuilder? builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.VividAspireInMemory>();
            builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });
            _app = await builder.BuildAsync();
            await _app.StartAsync();
            var httpClient = _app.CreateHttpClient("kernel");
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await _app.ResourceNotifications.WaitForResourceHealthyAsync(
                "kernel",
                cts.Token);
            await httpClient.PostAsync("/customer",
                new StringContent(
                    JsonSerializer.Serialize(new VividKernelCustomer { Id = "my-id", Secret = "my-secret" }),
                System.Text.Encoding.UTF8, "application/json"));
            await httpClient.PostAsync("/customer",
                new StringContent(
                    JsonSerializer.Serialize(new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" }),
                System.Text.Encoding.UTF8, "application/json"));
        }

        [TestMethod]
        public async Task SendPostToCustomerWithJsonShouldReturnThisNewCustomer()
        {
            // Arrange.
            var client = _app!.CreateHttpClient("kernel");
            VividKernelCustomer customer = new VividKernelCustomer { Id = "my-id-3", Secret = "my-secret-3" };
            string expected = JsonSerializer.Serialize(customer);

            // Act.
            HttpResponseMessage response = await client!.PostAsync("/customer", new StringContent(expected, System.Text.Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string actual = await response.Content.ReadAsStringAsync();

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task SendPostToCustomerShouldReturnOneMoreCustomer()
        {
            // Arrange.
            var client = _app!.CreateHttpClient("kernel");

            // Initial count.
            HttpResponseMessage responseCustomers = await client!.GetAsync("/customer");
            var customersArray = JsonSerializer.Deserialize<VividKernelCustomer[]>(await responseCustomers.Content.ReadAsStringAsync());
            int initialCount = customersArray != null ? customersArray.Length : 0;
            int expected = initialCount + 1;

            // Add a new one.
            VividKernelCustomer newCustomer = new VividKernelCustomer { Id = Guid.NewGuid().ToString(), Secret = Guid.NewGuid().ToString() };
            string newCustomerJson = JsonSerializer.Serialize(newCustomer);
            _ = await client.PostAsync(
                "/customer",
                new StringContent(newCustomerJson, System.Text.Encoding.UTF8, "application/json")
            );

            // Act.
            // New counter after added a new one.
            HttpResponseMessage responseCustomersAfterAdded = await client.GetAsync("/customer");
            var customersArrayAfterAdded = JsonSerializer.Deserialize<VividKernelCustomer[]>(await responseCustomersAfterAdded.Content.ReadAsStringAsync());
            int actual = customersArrayAfterAdded != null ? customersArrayAfterAdded.Length : 0;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        
        [TestMethod]
        public async Task SendGetToCustomerWithUnknownIdShouldReturn404()
        {
            // Arrange.
            var client = _app!.CreateHttpClient("kernel");
            int expected = new NotFoundResult().StatusCode;

            // Act.
            HttpResponseMessage response = await client!.GetAsync("/customer/my-id-not-found");
            int? actual = (int?)response.StatusCode;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task SendGetToCustomerWithMyIdParameterShouldReturnThatCustomer()
        {
            // Arrange
            var client = _app!.CreateHttpClient("kernel");
            VividKernelCustomer customer = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };
            string expected = JsonSerializer.Serialize(customer);

            // Act.
            HttpResponseMessage response = await client!.GetAsync("/customer/my-id");
            response.EnsureSuccessStatusCode();
            string actual = await response.Content.ReadAsStringAsync();

            // Assert.
            Assert.AreEqual(expected, actual);
        }
    }
}
