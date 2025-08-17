// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using System.Collections.Generic;
using System.Linq;
using Vivid.Kernel.DataAccess;
using Vivid.Kernel.Service;

namespace Vivid.Kernel.DataAccessInMemory
{
    /// <summary>
    /// Data access service in memory implementation.
    /// </summary>
    public class VividKernelDataAccessServiceInMemory : IVividKernelDataAccessService
    {
        /// <summary>
        /// Dictionary of customers, in memory.
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<string, VividKernelCustomer> _customers = [];

        /// <inheritdoc/>
        public void AddCustomer(VividKernelCustomer customer)
        {
            // Add a new customer to the list.
            _customers[customer.Id] = customer;
        }

        /// <inheritdoc/>
        public VividKernelCustomer? GetCustomer(string id)
        {
            // Check if the customer exists in the dictionary.
            // If it exists, return the customer; otherwise, return null.
            return _customers.TryGetValue(id, out VividKernelCustomer value) ? value : null;
        }

        /// <inheritdoc/>
        public VividKernelCustomer[] GetCustomers()
        {
            // Get the customers.
            VividKernelCustomer[] customers = _customers.Values.ToArray();

            // Return the customers.
            return customers;
        }
    }
}