using Microsoft.AspNetCore.Mvc;

using Vivid.Kernel.Service;

namespace Vivid.Kernel.WebserviceLib.Controllers;

/// <summary>
/// Initializes a new instance of the <see cref="CustomerController"/> class.
/// </summary>
/// <param name="kernelService">The kernel service used for customer operations.</param>
[ApiController] 
public class CustomerController(IKernelService kernelService) : ControllerBase
{
    [HttpPost]
    [Route("customer")]
    public ActionResult<Customer> PostCustomer(Customer customer)
    {
        // Add the customer to the kernel service.
        kernelService.AddCustomer(customer);

        // Retrieve the customer by ID.
        Customer? customerAdded = kernelService.GetCustomer(customer.Id);

        // Return the created customer.
        // TODO Missing coverage here.
        return customerAdded;
    }

    [HttpPost]
    [Route("customer/auto")]
    public ActionResult<Customer> PostCustomer()
    {
        // Create a new customer.
        Customer customer = kernelService.CreateNewCustomer();

        // Return the new customer.
        return customer;
    }

    [HttpGet]
    [Route("customer/{id}")]
    public ActionResult<Customer> GetCustomer(string id)
    {
        // Retrieve the customer from the kernel service.
        Customer? customer = kernelService.GetCustomer(id);

        // Check if the customer was found.
        if (customer == null)
        {
            // Customer not found.
            return NotFound();
        }

        // Return the customer.
        // TODO Missing coverage here.
        return customer;
    }

    [HttpGet]
    [Route("customer")]
    public ActionResult<Customer[]> GetCustomers()
    {
        // Retrieve all customers from the kernel service.
        Customer[] customers = kernelService.GetCustomers();

        // Return the list of customers.
        return customers;
    }
}   