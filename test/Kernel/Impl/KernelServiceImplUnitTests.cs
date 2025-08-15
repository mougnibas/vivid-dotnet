// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using VividKernelService.Model;
using VividKernelServiceImpl;

namespace VividKernelTest.Impl;

/// <summary>
/// Unit tests of ``KernelServiceImpl`` class.
/// </summary>
[TestClass]
public sealed class KernelServiceImplUnitTests
{
    private KernelService service = new KernelService();

    [TestInitialize]
    public void SetUp()
    {
        // Add a few customers.
        service.AddCustomer(new Customer { Id = "my-id", Secret = "my-secret" });
        service.AddCustomer(new Customer { Id = "my-id-2", Secret = "my-secret-2" });
    }

    [TestMethod]
    public void DefaultConstructorShouldNotThrowException()
    {
        // Arrange and Act.
        KernelService service = new KernelService();

        // Assert.
        Assert.IsNotNull(service);
    }

    [TestMethod]
    public void AddCustomerShouldThenGetCustomerShouldReturnThisCustomer()
    {
        // Arrange.
        Customer expected = new Customer { Id = "my-id-3", Secret = "my-secret-3" };

        // Act.
        service.AddCustomer(expected);
        Customer? actual = service.GetCustomer("my-id-3");

        // Assert.
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreateNewCustomerShouldReturnAnotherCustomer()
    {
        // Arrange.
        int numberOfCustomersBefore = service.GetCustomers().Length;
        int expected = numberOfCustomersBefore + 1;
        service.CreateNewCustomer();

        // Act.
        int actual = service.GetCustomers().Length;

        // Assert.
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetCustomerByIdWithMyIdShouldReturnThisCustomer()
    {
        // Arrange.
        Customer expected = new Customer { Id = "my-id", Secret = "my-secret" };

        // Act.
        Customer? actual = service.GetCustomer("my-id");

        // Assert.
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetCustomerByIdWithIdTwoShouldReturnThisCustomer()
    {
        // Arrange.
        Customer expected = new Customer { Id = "my-id-2", Secret = "my-secret-2" };

        // Act.
        Customer? actual = service.GetCustomer("my-id-2");

        // Assert.
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetCustomerByIdWithIdThreeShouldReturnNull()
    {
        // Arrange.
        Customer? expected = null;

        // Act.
        Customer? actual = service.GetCustomer("my-id-3");

        // Assert.
        Assert.AreEqual(expected, actual);
    }
}