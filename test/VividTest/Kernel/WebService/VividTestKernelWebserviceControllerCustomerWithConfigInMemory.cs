// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vivid.Kernel.DataAccessInMemory;
using Vivid.Kernel.Service;
using Vivid.Kernel.ServiceCore;
using Vivid.Kernel.Webservice;

namespace VividTest.Kernel.Webservice
{
    /// <summary>
    /// Unit tests of ``CustomerController`` class.
    /// </summary>
    [TestClass]
    public sealed class VividTestKernelWebserviceControllerCustomerWithConfigInMemory
    {
        private VividKernelWebserviceControllerCustomer? _controller;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the in-memory data access service and kernel service.
            VividKernelDataAccessServiceInMemory dataAccessService = new VividKernelDataAccessServiceInMemory();
            IVividKernelService kernelService = new VividKernelServiceCore(dataAccessService);

            // Populate initial data
            dataAccessService.AddCustomer(new VividKernelCustomer { Id = "my-id", Secret = "my-secret" });
            dataAccessService.AddCustomer(new VividKernelCustomer { Id = "my-id-2", Secret = "my-secret-2" });

            // Initialize the customer controller.
            _controller = new VividKernelWebserviceControllerCustomer(kernelService);
        }

        [TestMethod]
        public void SendPostToCustomerWithJsonShouldReturnThisNewCustomer()
        {
            // Arrange.
            VividKernelCustomer expected = new VividKernelCustomer { Id = "my-id-3", Secret = "my-secret-3" };

            // Act.
            VividKernelCustomer? actual = _controller?.PostCustomer(expected).Value;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SendPostToCustomerShouldReturnOneMoreCustomer()
        {
            // Arrange.
            int initialCount = _controller?.GetCustomers()?.Value?.Length ?? 0;
            int expected = initialCount + 1;

            // Act.
            _controller?.PostCustomer();
            int actual = _controller?.GetCustomers()?.Value?.Length ?? 0;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SendGetToCustomerWithUnknownIdShouldReturn404()
        {
            // Arrange.
            int expected = new NotFoundResult().StatusCode;

            // Act.
            ActionResult<VividKernelCustomer> actualResult = _controller!.GetCustomer("my-id-not-found");
            int? actual = (actualResult.Result as StatusCodeResult)?.StatusCode;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SendGetToCustomerWithMyIdParameterShouldReturnThatCustomer()
        {
            // Arrange.
            VividKernelCustomer expected = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };

            // Act.
            VividKernelCustomer? actual = _controller?.GetCustomer("my-id").Value;

            // Assert.
            Assert.AreEqual(expected, actual);
        }
    }
}
