
// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using System.Collections.Generic;
using VividKernelService.Model;
using VividKernelService.Service;

namespace VividKernelServiceImpl
{
    /// <summary>
    /// Kernel service implementation.
    /// </summary>
    public class KernelService : IKernelService
    {
        /// <summary>
        /// Dictionary of customers, in memory.
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<string, Customer> _customers = [];

        /// <inheritdoc/>
        public void AddCustomer(Customer customer)
        {
            // Add a new customer to the list.
            _customers.Add(customer.Id, customer);
        }

        /// <inheritdoc/>
        public Customer CreateNewCustomer()
        {
            // Create a new customer.
            string customerId = Guid.NewGuid().ToString();
            string customerSecret = Guid.NewGuid().ToString();
            var customer = new Customer { Id = customerId, Secret = customerSecret };

            // Add the new customer to the list.
            _customers.Add(customer.Id, customer);

            // Return the new customer.
            return customer;
        }

        /// <inheritdoc/>
        public Customer? GetCustomer(string id)
        {
            // Check if the customer exists in the dictionary.
            // If it exists, return the customer; otherwise, return null.
            return _customers.TryGetValue(id, out Customer value) ? value : null;
        }

        /// <inheritdoc/>
        public Customer[] GetCustomers()
        {
            // Get the customers.
            Customer[] customers = new List<Customer>(_customers.Values).ToArray();

            // Return the customers.
            return customers;
        }
    }
}