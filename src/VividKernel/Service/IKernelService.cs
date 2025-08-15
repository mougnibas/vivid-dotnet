
// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

namespace Vivid.Kernel.Service
{

    public interface IKernelService
    {
        /// <summary>
        /// Adds a customer.
        /// </summary>
        /// <param name="customer">The customer to add.</param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Create, then return a new customer.
        /// </summary>
        /// <returns>The newly created customer.</returns>
        Customer CreateNewCustomer();

        /// <summary>
        /// Retrieves a customer by their identifier, if one exists.
        /// </summary>
        /// <param name="id">Identifier of a customer</param>
        /// <returns>The customer, if found; otherwise, null.</returns>
        Customer? GetCustomer(string id);

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <remarks>
        /// The returned array is never null; it will be empty if there are no customers.
        /// </remarks>
        /// <returns>An array of all customers, or an empty array if none exist.</returns>
        Customer[] GetCustomers();
    }
}