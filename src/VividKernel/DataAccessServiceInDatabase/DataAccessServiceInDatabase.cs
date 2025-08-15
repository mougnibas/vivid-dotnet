
// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System.Linq;
using System.Collections.Generic;
using Vivid.Kernel.Service;
using Vivid.Kernel.DataAccessService.InDatabase.Model;

namespace Vivid.Kernel.DataAccessService.InDatabase
{
    /// <summary>
    /// Data access service backed by a database implementation.
    /// </summary>
    public class DataAccessServiceInDatabase(VividDbContext dbContext) : IDataAccessService
    {
        /// <inheritdoc/>
        public void AddCustomer(Customer customer)
        {
            // Create a CustomerModel from customer.
            CustomerModel customerModel = new CustomerModel(customer.Id, customer.Secret);

            // Add the CustomerModel to the DbContext.
            dbContext.Customers.Add(customerModel);

            // Save changes to the database.
            dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public Customer? GetCustomer(string id)
        {
            // Try to find the customer in the database.
            CustomerModel? customerModel = dbContext.Customers.Find(id);

            // If the customer is not found, return null.
            if (customerModel == null)
            {
                return null;
            }

            // Create a customer from CustomerModel.
            Customer customer = new Customer
            {
                Id = customerModel.Id,
                Secret = customerModel.Secret
            };

            // Return it.
            return customer;
        }

        /// <inheritdoc/>
        public Customer[] GetCustomers()
        {
            // Get the customer models.
            CustomerModel[] customersModel = dbContext.Customers.ToArray();

            // Convert the CustomerModels to Customers.
            List<Customer> customersList = new List<Customer>(customersModel.Length);
            foreach (CustomerModel currentCustomerModel in customersModel)
            {
                Customer customer = new Customer
                {
                    Id = currentCustomerModel.Id,
                    Secret = currentCustomerModel.Secret
                };
                customersList.Add(customer);
            }

            // Get the final array of customers.
            Customer[] customers = customersList.ToArray();

            // Return the result.
            return customers;
        }
    }
}