using System.Text.Json;
using Microsoft.Extensions.Options;
using TestingWebApp.Models;
using TestingWebApp.Options;

namespace TestingWebApp.Repositories;

public class CustomerJsonRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    private readonly IWebHostEnvironment _environment;
    private readonly CustomerDatabaseOptions _options;
    private readonly ILogger<CustomerJsonRepository> _logger;

    public CustomerJsonRepository(
        IWebHostEnvironment environment,
        IOptions<CustomerDatabaseOptions> options,
        ILogger<CustomerJsonRepository> logger)
    {
        _environment = environment;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        var filePath = ResolveFilePath();

        try
        {
            await using var stream = File.OpenRead(filePath);
            var customers = await JsonSerializer.DeserializeAsync<List<Customer>>(stream, JsonOptions);
            return customers ?? [];
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError(ex, "Customer JSON database file was not found at {FilePath}.", filePath);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse the customer JSON database file at {FilePath}.", filePath);
            throw;
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "Failed to read the customer JSON database file at {FilePath}.", filePath);
            throw;
        }
    }

    public async Task<Customer?> GetByNrcAsync(string nrc)
    {
        var normalizedNrc = NormalizeNrc(nrc);
        var customers = await GetAllCustomersAsync();

        return customers.FirstOrDefault(customer =>
            string.Equals(NormalizeNrc(customer.Nrc), normalizedNrc, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Customer?> GetByCustomerIdAsync(string customerId)
    {
        var normalizedId = customerId.Trim();
        var customers = await GetAllCustomersAsync();

        return customers.FirstOrDefault(customer =>
            string.Equals(customer.CustomerId.Trim(), normalizedId, StringComparison.OrdinalIgnoreCase));
    }

    private string ResolveFilePath()
    {
        var configuredPath = _options.FilePath;

        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            configuredPath = "Data/customers.json";
        }

        if (Path.IsPathRooted(configuredPath) && File.Exists(configuredPath))
        {
            return configuredPath;
        }

        var contentRootPath = Path.Combine(_environment.ContentRootPath, configuredPath);
        if (File.Exists(contentRootPath))
        {
            return contentRootPath;
        }

        return Path.Combine(AppContext.BaseDirectory, configuredPath);
    }

    private static string NormalizeNrc(string nrc)
    {
        var value = nrc.Trim();

        // ASP.NET leaves %2F encoded in the path; decode so 12/ABC(N)123456 matches.
        for (var i = 0; i < 2; i++)
        {
            var decoded = Uri.UnescapeDataString(value);
            if (decoded == value)
            {
                break;
            }

            value = decoded;
        }

        return value;
    }
}
