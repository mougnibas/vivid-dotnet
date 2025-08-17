// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vivid.Kernel.DataAccessInMemory;
using Vivid.Kernel.Service;
using Vivid.Kernel.ServiceCore;

namespace VividTest.Kernel.ServiceCore
{
    /// <summary>
    /// Unit tests of ``KernelServiceImpl`` class.
    /// </summary>
    [TestClass]
    public sealed class VividTestKernelServiceCore
    {
        private VividKernelServiceCore service = new VividKernelServiceCore(new VividKernelDataAccessServiceInMemory());

        [TestInitialize]
        public void SetUp()
        {
            // Add a few customers.
            service.AddCustomer(new VividKernelCustomer { Id = "my-id", Secret = "my-secret" });
            service.AddCustomer(new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" });
        }

        [TestMethod]
        public void AddCustomerShouldThenGetCustomerShouldReturnThisCustomer()
        {
            // Arrange.
            VividKernelCustomer expected = new VividKernelCustomer { Id = "my-id-3", Secret = "my-secret-3" };

            // Act.
            service.AddCustomer(expected);
            VividKernelCustomer? actual = service.GetCustomer("my-id-3");

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
            VividKernelCustomer expected = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };

            // Act.
            VividKernelCustomer? actual = service.GetCustomer("my-id");

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCustomerByIdWithIdTwoShouldReturnThisCustomer()
        {
            // Arrange.
            VividKernelCustomer expected = new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" };

            // Act.
            VividKernelCustomer? actual = service.GetCustomer("my-id-2");

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCustomerByIdWithIdThreeShouldReturnNull()
        {
            // Arrange.
            VividKernelCustomer? expected = null;

            // Act.
            VividKernelCustomer? actual = service.GetCustomer("my-id-3");

            // Assert.
            Assert.AreEqual(expected, actual);
        }
    }
}
