using TestingWebApp.Models;
using TestingWebApp.Repositories;

namespace TestingWebApp.Services;

public class CustomerService
{
    private readonly CustomerJsonRepository _repository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(CustomerJsonRepository repository, ILogger<CustomerService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string?> GetCustomerIdByNrcAsync(string nrc)
    {
        var customer = await GetCustomerByNrcAsync(nrc);
        return customer?.CustomerId;
    }

    public async Task<Customer?> GetCustomerByNrcAsync(string nrc)
    {
        var normalizedNrc = Uri.UnescapeDataString(nrc.Trim());
        _logger.LogInformation("Searching customer by NRC: {Nrc}", normalizedNrc);

        var customer = await _repository.GetByNrcAsync(normalizedNrc);
        if (customer is null)
        {
            _logger.LogInformation("Customer not found for the provided NRC.");
            return null;
        }

        _logger.LogInformation("Customer found: {CustomerId}", customer.CustomerId);
        return customer;
    }

    public async Task<IReadOnlyList<CustomerIdNrcResponse>> GetAllCustomerIdsAndNrcsAsync()
    {
        _logger.LogInformation("Listing all customer IDs and NRCs.");

        var customers = await _repository.GetAllCustomersAsync();
        return customers
            .Select(customer => new CustomerIdNrcResponse
            {
                CustomerId = customer.CustomerId,
                Nrc = customer.Nrc
            })
            .ToList();
    }

    public async Task<Customer?> GetCustomerByIdAsync(string customerId)
    {
        var normalizedId = customerId.Trim();
        _logger.LogInformation("Searching customer by customer ID: {CustomerId}", normalizedId);

        var customer = await _repository.GetByCustomerIdAsync(normalizedId);
        if (customer is null)
        {
            _logger.LogInformation("Customer not found for customer ID: {CustomerId}", normalizedId);
            return null;
        }

        _logger.LogInformation("Customer found: {CustomerId}", customer.CustomerId);
        return customer;
    }
}
