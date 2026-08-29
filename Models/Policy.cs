namespace TestingWebApp.Models;

public class Policy
{
    public string PolicyId { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public PolicyParty PolicyOwner { get; set; } = new();
    public PolicyParty LifeInsured { get; set; } = new();
    public List<Beneficiary> Beneficiaries { get; set; } = [];
    public Premium Premium { get; set; } = new();
    public Coverage Coverage { get; set; } = new();
    public DateOnly? PolicyStartDate { get; set; }
    public DateOnly? PolicyEndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class PolicyParty
{
    public string CustomerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
}

public class Beneficiary
{
    public string Name { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public int Percentage { get; set; }
}

public class Premium
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public DateOnly? NextPaymentDate { get; set; }
}

public class Coverage
{
    public decimal SumAssured { get; set; }
    public string Currency { get; set; } = string.Empty;
}
