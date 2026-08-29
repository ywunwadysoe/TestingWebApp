namespace TestingWebApp.Models;

public class Customer
{
    public string CustomerId { get; set; } = string.Empty;
    public string Nrc { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Occupation { get; set; } = string.Empty;
    public decimal MonthlyIncome { get; set; }
    public string Location { get; set; } = string.Empty;
    public string PreferredLanguage { get; set; } = string.Empty;
    public string PreferredContactChannel { get; set; } = string.Empty;
    public List<Policy> Policies { get; set; } = [];
    public List<PaymentHistory> PaymentHistory { get; set; } = [];
    public CustomerBehaviour CustomerBehaviour { get; set; } = new();
    public List<Notification> Notifications { get; set; } = [];
    public CustomerServiceHistory CustomerService { get; set; } = new();
}
