// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System.Linq;
using System.Collections.Generic;
using Vivid.Kernel.Service;
using Vivid.Kernel.DataAccess;

namespace Vivid.Kernel.DataAccessInDatabase
{
    /// <summary>
    /// Data access service backed by a database implementation.
    /// </summary>
    public class VividKernelDataAccessServiceInDatabase(VividKernelDbContext dbContext) : IVividKernelDataAccessService
    {
        /// <inheritdoc/>
        public void AddCustomer(VividKernelCustomer customer)
        {
            // Create a CustomerModel from customer.
            VividKernelCustomerModel customerModel = new VividKernelCustomerModel(customer.Id, customer.Secret);

            // Add the CustomerModel to the DbContext.
            dbContext.Customers.Add(customerModel);

            // Save changes to the database.
            dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public VividKernelCustomer? GetCustomer(string id)
        {
            // Try to find the customer in the database.
            VividKernelCustomerModel? customerModel = dbContext.Customers.Find(id);

            // If the customer is not found, return null.
            if (customerModel == null)
            {
                return null;
            }

            // Create a customer from CustomerModel.
            VividKernelCustomer customer = new VividKernelCustomer
            {
                Id = customerModel.Id,
                Secret = customerModel.Secret
            };

            // Return it.
            return customer;
        }

        /// <inheritdoc/>
        public VividKernelCustomer[] GetCustomers()
        {
            // Get the customer models.
            VividKernelCustomerModel[] customersModel = dbContext.Customers.ToArray();

            // Convert the CustomerModels to Customers.
            List<VividKernelCustomer> customersList = new List<VividKernelCustomer>(customersModel.Length);
            foreach (VividKernelCustomerModel currentCustomerModel in customersModel)
            {
                VividKernelCustomer customer = new VividKernelCustomer
                {
                    Id = currentCustomerModel.Id,
                    Secret = currentCustomerModel.Secret
                };
                customersList.Add(customer);
            }

            // Get the final array of customers.
            VividKernelCustomer[] customers = customersList.ToArray();

            // Return the result.
            return customers;
        }
    }
}