using Microsoft.AspNetCore.Mvc;
using TestingWebApp.Models;

namespace TestingWebApp.Controllers;

[ApiController]
[Route("api/customers")]
[Produces("application/json")]
public class CustomerController : ControllerBase
{
    private readonly Services.CustomerService _customerService;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(Services.CustomerService customerService, ILogger<CustomerController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Returns every customer ID and NRC.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerIdNrcResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCustomerIdsAndNrcs()
    {
        try
        {
            var customers = await _customerService.GetAllCustomerIdsAndNrcsAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while listing customer IDs and NRCs.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An unexpected error occurred."
            });
        }
    }

    /// <summary>
    /// Returns the complete customer record for an NRC.
    /// </summary>
    [HttpGet("details/by-nrc/{*nrc}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerByNrc(string nrc)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nrc))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "NRC is required."
                });
            }

            var customer = await _customerService.GetCustomerByNrcAsync(nrc);
            if (customer is null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Customer not found for the provided NRC."
                });
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while searching customer details by NRC.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An unexpected error occurred."
            });
        }
    }

    /// <summary>
    /// Finds a customer ID by NRC.
    /// </summary>
    [HttpGet("by-nrc/{*nrc}")]
    [ProducesResponseType(typeof(CustomerIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerIdByNrc(string nrc)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nrc))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "NRC is required."
                });
            }

            var customerId = await _customerService.GetCustomerIdByNrcAsync(nrc);
            if (customerId is null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Customer not found for the provided NRC."
                });
            }

            return Ok(new CustomerIdResponse { CustomerId = customerId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while searching customer by NRC.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An unexpected error occurred."
            });
        }
    }

    /// <summary>
    /// Returns the complete customer record for a customer ID.
    /// </summary>
    [HttpGet("{customerId}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerById(string customerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Customer ID is required."
                });
            }

            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer is null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Customer not found."
                });
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while searching customer by customer ID.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An unexpected error occurred."
            });
        }
    }
}
