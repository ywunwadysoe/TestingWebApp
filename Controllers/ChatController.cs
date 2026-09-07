using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TestingWebApp.Models;
using TestingWebApp.Services;

namespace TestingWebApp.Controllers;

[ApiController]
[Route("api/chat")]
[Produces("application/json")]
public class ChatController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CustomerService _customerService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChatController> _logger;

    public ChatController(
        IHttpClientFactory httpClientFactory,
        CustomerService customerService,
        IConfiguration configuration,
        ILogger<ChatController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _customerService = customerService;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(ChatRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new ErrorResponse { Message = "Message is required." });
        }

        var apiKey = _configuration["Groq:ApiKey"];
        var model = _configuration["Groq:Model"];
        var baseUrl = _configuration["Groq:BaseUrl"];
        if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(baseUrl))
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new ErrorResponse
            {
                Message = "The AI service is not configured on the backend."
            });
        }

        Customer? customer = null;
        if (!string.IsNullOrWhiteSpace(request.CustomerId))
        {
            customer = await _customerService.GetCustomerByIdAsync(request.CustomerId);
        }

        var customerContext = customer is null
            ? "No customer profile was supplied."
            : JsonSerializer.Serialize(customer);
        var payload = new
        {
            model,
            temperature = 0.2,
            messages = new object[]
            {
                new { role = "system", content = "You are a concise customer service assistant. Use the customer profile when answering. Never invent customer data. Customer profile: " + customerContext },
                new { role = "user", content = request.Message.Trim() }
            }
        };

        try
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, baseUrl);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var client = _httpClientFactory.CreateClient();
            using var response = await client.SendAsync(httpRequest, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("AI service returned {StatusCode}: {ResponseBody}", response.StatusCode, responseBody);
                return StatusCode(StatusCodes.Status502BadGateway, new ErrorResponse
                {
                    Message = "The AI service could not process the request."
                });
            }

            using var document = JsonDocument.Parse(responseBody);
            var content = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return Ok(new { message = content ?? "The AI service returned an empty response." });
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while calling the AI service.");
            return StatusCode(StatusCodes.Status502BadGateway, new ErrorResponse
            {
                Message = "The AI service could not be reached."
            });
        }
    }
}

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
    public string? CustomerId { get; set; }
}