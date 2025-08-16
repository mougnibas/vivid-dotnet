using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Customer> PostCustomer(Customer customer)
    {
        // Add the customer to the kernel service.
        kernelService.AddCustomer(customer);

        // Create the result.
        ActionResult<Customer> result = Ok(customer);

        // Return the given customer.
        return result;
    }

    [HttpPost]
    [Route("customer/auto")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Customer> PostCustomer()
    {
        // Create a new customer.
        Customer customer = kernelService.CreateNewCustomer();

        // Create the result.
        ActionResult<Customer> result = Ok(customer);

        // Return the new customer.
        return result;
    }

    [HttpGet]
    [Route("customer/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Customer> GetCustomer(string id)
    {
        // Retrieve the customer from the kernel service.
        Customer? customer = kernelService.GetCustomer(id);

        // Check if the customer was found.
        if (customer == null)
        {
            // Create the result.
            ActionResult<Customer> notFoundResult = NotFound();

            // Return the result.
            return notFoundResult;
        }

        // Create the result.
        ActionResult<Customer> result = Ok(customer);

        // Return the result.
        return result;
    }

    [HttpGet]
    [Route("customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Customer[]> GetCustomers()
    {
        // Retrieve all customers from the kernel service.
        Customer[] customers = kernelService.GetCustomers();

        // Create the result.
        ActionResult<Customer[]> result = Ok(customers);

        // Return the list of customers.
        return result;
    }
}   