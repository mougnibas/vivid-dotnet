// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Vivid.Kernel.DataAccessService.InDatabase;
using Vivid.Kernel.Service;

namespace VividTest.VividKernel.DataAccessServiceInDatabaseTest;

/// <summary>
/// Unit tests of ``KernelDataAccessServiceServiceInDatabase`` class.
/// </summary>
[TestClass]
public sealed class DataAccessServiceInDatabaseTest
{
    private DataAccessServiceInDatabase? service = null;

    private VividDbContext? dbContext;

    [TestInitialize]
    public void SetUp()
    {
        // Setup a DB Context in-memory for testing.
        DbContextOptions<VividDbContext> options = new DbContextOptionsBuilder<VividDbContext>()
            .UseInMemoryDatabase(databaseName: "KernelDb")
            .Options;
        dbContext = new VividDbContext(options);

        // Reference the service with the DbContext.
        service = new DataAccessServiceInDatabase(dbContext);

        // Add a few customers.
        service.AddCustomer(new Customer { Id = "my-id", Secret = "my-secret" });
        service.AddCustomer(new Customer { Id = "my-id-2", Secret = "my-secret-2" });
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
        Customer expected = new Customer { Id = "my-id-3", Secret = "my-secret-3" };

        // Act.
        service!.AddCustomer(expected);
        Customer? actual = service.GetCustomer("my-id-3");

        // Assert.
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetCustomerByIdWithMyIdShouldReturnThisCustomer()
    {
        // Arrange.
        Customer expected = new Customer { Id = "my-id", Secret = "my-secret" };

        // Act.
        Customer? actual = service!.GetCustomer("my-id");

        // Assert.
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetCustomerByIdWithMyIdTwoShouldReturnThisCustomer()
    {
        // Arrange.
        Customer expected = new Customer { Id = "my-id-2", Secret = "my-secret-2" };

        // Act.
        Customer? actual = service!.GetCustomer("my-id-2");

        // Assert.
        Assert.AreEqual(expected, actual);
    }



    [TestMethod]
    public void GetCustomerByIdWithMyIdThreeShouldReturnNull()
    {
        // Arrange.
        Customer? expected = null;

        // Act.
        Customer? actual = service!.GetCustomer("my-id-3");

        // Assert.
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetCustomersShouldReturnAllCustomers()
    {
        // Arrange.
        Customer[] expected = new Customer[]
        {
            new Customer { Id = "my-id", Secret = "my-secret" },
            new Customer { Id = "my-id-2", Secret = "my-secret-2" }
        };

        // Act.
        Customer[] actual = service!.GetCustomers();

        // Assert.
        CollectionAssert.AreEqual(expected, actual);
    }
}
