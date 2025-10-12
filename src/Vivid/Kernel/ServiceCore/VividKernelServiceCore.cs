// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using System.Collections.Generic;
using Vivid.Kernel.DataAccess;
using Vivid.Kernel.Service;

namespace Vivid.Kernel.ServiceCore
{
    /// <summary>
    /// Kernel service implementation.
    /// </summary>
    /// <param name="dataAccessService">Reference to a data access service.</param>
    public class VividKernelServiceCore(IVividKernelDataAccessService dataAccessService) : IVividKernelService
    {
        /// <summary>
        /// Generates a secure random secret.
        /// </summary>
        /// <param name="length">The length of the secret.</param>
        /// <returns>A secure random secret.</returns>
        private static string GenerateSecureSecret(int length = 32)
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
       
        /// <inheritdoc/>
        public void AddCustomer(VividKernelCustomer customer)
        {
            // Add a new customer to the list.
            dataAccessService.AddCustomer(customer);
        }

        /// <inheritdoc/>
        public VividKernelCustomer CreateNewCustomer()
        {
            // Create a new customer.
            string customerId = Guid.NewGuid().ToString();
            string customerSecret = GenerateSecureSecret();
            var customer = new VividKernelCustomer { Id = customerId, Secret = customerSecret };

            // Add the new customer to the data.
            dataAccessService.AddCustomer(customer);

            // Return the new customer.
            return customer;
        }

        /// <inheritdoc/>
        public VividKernelCustomer? GetCustomer(string id)
        {
            return dataAccessService.GetCustomer(id);
        }

        /// <inheritdoc/>
        public VividKernelCustomer[] GetCustomers()
        {
            // Get the customers.
            VividKernelCustomer[] customers = dataAccessService.GetCustomers();

            // Return the customers.
            return customers;
        }
    }
}