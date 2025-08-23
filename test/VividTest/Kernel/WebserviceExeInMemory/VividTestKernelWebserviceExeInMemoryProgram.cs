// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using Vivid.Kernel.DataAccessInMemory;
using Vivid.Kernel.Service;
using Vivid.Kernel.ServiceCore;
using Vivid.Kernel.Webservice;
using Vivid.Kernel.WebserviceExeInMemory;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;

namespace VividTest.Kernel.WebserviceExeInMemory
{
    [TestClass]
    public sealed class VividTestKernelWebserviceExeInMemoryProgram
    {
        private WebApplicationFactory<Vivid.Kernel.WebserviceExeInMemory.Program> _factory;

        private static HttpClient _client;

        [TestInitialize]
        public async Task Setup()
        {
            _factory = new WebApplicationFactory<Vivid.Kernel.WebserviceExeInMemory.Program>();
            _client = _factory.CreateClient();

            // Populate initial data
            await _client.PostAsync("/customer",
                new StringContent(
                    JsonSerializer.Serialize(new VividKernelCustomer { Id = "my-id", Secret = "my-secret" }),
                System.Text.Encoding.UTF8, "application/json"));
            await _client.PostAsync("/customer",
                new StringContent(
                    JsonSerializer.Serialize(new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" }),
                System.Text.Encoding.UTF8, "application/json"));
        }

        [TestCleanup]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }


        [TestMethod]
        public async Task SendPostToCustomerWithJsonShouldReturnThisNewCustomer()
        {
            // Arrange.
            VividKernelCustomer customer = new VividKernelCustomer { Id = "my-id-3", Secret = "my-secret-3" };
            string expected = JsonSerializer.Serialize(customer);

            // Act.
            HttpResponseMessage response = await _client.PostAsync("/customer", new StringContent(expected, System.Text.Encoding.UTF8, "application/json"));
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
            HttpResponseMessage responseCustomers = await _client.GetAsync("/customer");
            int initialCount = JsonSerializer.Deserialize<VividKernelCustomer[]>(await responseCustomers.Content.ReadAsStringAsync()).Length;
            int expected = initialCount + 1;

            // Add a new one.
            VividKernelCustomer newCustomer = new VividKernelCustomer { Id = "my-id-3", Secret = "my-secret-3" };
            string newCustomerJson = JsonSerializer.Serialize(newCustomer);
            _ = await _client.PostAsync(
                "/customer",
                new StringContent(newCustomerJson, System.Text.Encoding.UTF8, "application/json")
            );

            // Act.
            // New counter after added a new one.
            HttpResponseMessage responseCustomersAfterAdded = await _client.GetAsync("/customer");
            int actual = JsonSerializer.Deserialize<VividKernelCustomer[]>(await responseCustomersAfterAdded.Content.ReadAsStringAsync()).Length;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        
        [TestMethod]
        public async Task SendGetToCustomerWithUnknownIdShouldReturn404()
        {
            // Arrange.
            int expected = new NotFoundResult().StatusCode;

            // Act.
            HttpResponseMessage response = await _client.GetAsync("/customer/my-id-not-found");
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
            HttpResponseMessage response = await _client.GetAsync("/customer/my-id");
            response.EnsureSuccessStatusCode();
            string actual = await response.Content.ReadAsStringAsync();

            // Assert.
            Assert.AreEqual(expected, actual);
        }
    }
}
