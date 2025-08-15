
// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using System.Collections.Generic;
using Vivid.Kernel.DataAccessService;

namespace Vivid.Kernel.Service.Core
{
    /// <summary>
    /// Kernel service implementation.
    /// </summary>
    /// <param name="dataAccessService">Reference to a data access service.</param>
    public class KernelServiceCore(IDataAccessService dataAccessService) : IKernelService
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
        public void AddCustomer(Customer customer)
        {
            // Add a new customer to the list.
            dataAccessService.AddCustomer(customer);
        }

        /// <inheritdoc/>
        public Customer CreateNewCustomer()
        {
            // Create a new customer.
            string customerId = Guid.NewGuid().ToString();
            string customerSecret = GenerateSecureSecret();
            var customer = new Customer { Id = customerId, Secret = customerSecret };

            // Add the new customer to the data.
            dataAccessService.AddCustomer(customer);

            // Return the new customer.
            return customer;
        }

        /// <inheritdoc/>
        public Customer? GetCustomer(string id)
        {
            return dataAccessService.GetCustomer(id);
        }

        /// <inheritdoc/>
        public Customer[] GetCustomers()
        {
            // Get the customers.
            Customer[] customers = dataAccessService.GetCustomers();

            // Return the customers.
            return customers;
        }
    }
}