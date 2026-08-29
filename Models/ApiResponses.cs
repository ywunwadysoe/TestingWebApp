namespace TestingWebApp.Models;

public class CustomerIdResponse
{
    public string CustomerId { get; set; } = string.Empty;
}

public class CustomerIdNrcResponse
{
    public string CustomerId { get; set; } = string.Empty;
    public string Nrc { get; set; } = string.Empty;
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}
