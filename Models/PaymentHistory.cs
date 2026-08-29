namespace TestingWebApp.Models;

public class PaymentHistory
{
    public string PaymentId { get; set; } = string.Empty;
    public string PolicyId { get; set; } = string.Empty;
    public DateOnly? Date { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
