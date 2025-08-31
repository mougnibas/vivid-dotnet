// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Vivid.Kernel.Service;
using Vivid.Kernel.DataAccessInDatabase;
using System;

namespace VividTest.Kernel.DataAccessInDatabase
{
    /// <summary>
    /// Unit tests of ``KernelDataAccessServiceServiceInDatabase`` class.
    /// </summary>
    [TestClass]
    public sealed class VividTestKernelDataAccessServiceInDatabase
    {
        private VividKernelDataAccessServiceInDatabase? service = null;

        private VividKernelDbContext? dbContext;

        [TestInitialize]
        public void SetUp()
        {
            // Setup a DB Context in-memory for testing.
            string randomDbName = string.Format("KernelDb-{0}", Guid.NewGuid().ToString("N").Substring(0, 16));
            DbContextOptions<VividKernelDbContext> options = new DbContextOptionsBuilder<VividKernelDbContext>()
                .UseInMemoryDatabase(databaseName: randomDbName)
                .Options;
            dbContext = new VividKernelDbContext(options);

            // Reference the service with the DbContext.
            service = new VividKernelDataAccessServiceInDatabase(dbContext);

            // Add a few customers.
            service.AddCustomer(new VividKernelCustomer { Id = "my-id", Secret = "my-secret" });
            service.AddCustomer(new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" });
        }

        [TestCleanup]
        public void TearDown()
        {
            dbContext?.Database.EnsureDeleted();
        }

        [TestMethod]
        public void AddCustomerShouldThenGetCustomerIdThreeShouldReturnThisCustomer()
        {
            // Arrange.
            VividKernelCustomer expected = new VividKernelCustomer { Id = "my-id-3", Secret = "my-secret-3" };

            // Act.
            service!.AddCustomer(expected);
            VividKernelCustomer? actual = service.GetCustomer("my-id-3");

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCustomerByIdWithMyIdShouldReturnThisCustomer()
        {
            // Arrange.
            VividKernelCustomer expected = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };

            // Act.
            VividKernelCustomer? actual = service!.GetCustomer("my-id");

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCustomerByIdWithMyIdTwoShouldReturnThisCustomer()
        {
            // Arrange.
            VividKernelCustomer expected = new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" };

            // Act.
            VividKernelCustomer? actual = service!.GetCustomer("my-id-2");

            // Assert.
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        public void GetCustomerByIdWithMyIdThreeShouldReturnNull()
        {
            // Arrange.
            VividKernelCustomer? expected = null;

            // Act.
            VividKernelCustomer? actual = service!.GetCustomer("my-id-3");

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCustomersShouldReturnAllCustomers()
        {
            // Arrange.
            VividKernelCustomer[] expected = new VividKernelCustomer[]
            {
            new VividKernelCustomer { Id = "my-id", Secret = "my-secret" },
            new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" }
            };

            // Act.
            VividKernelCustomer[] actual = service!.GetCustomers();

            // Assert.
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
