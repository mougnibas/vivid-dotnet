using Microsoft.AspNetCore.Mvc;

using Vivid.Kernel.Service;

namespace Vivid.Kernel.WebserviceLib.Controllers;

/// <summary>
/// HTTP controller for managing customers.
/// </summary>
/// <param name="kernelService">The kernel service.</param>
[ApiController]
public class CustomerController(IKernelService kernelService) : ControllerBase
{
    [HttpGet]
    [Route("customer")]
    public ActionResult<Customer[]> GetAll()
    {
        Customer[] customers = kernelService.GetCustomers();
        return Ok(customers);
    }
}   