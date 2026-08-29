namespace TestingWebApp.Models;

public class CustomerServiceHistory
{
    public List<Inquiry> PreviousInquiries { get; set; } = [];
    public List<Complaint> Complaints { get; set; } = [];
    public List<ServiceRequest> ServiceRequests { get; set; } = [];
}

public class Inquiry
{
    public string InquiryId { get; set; } = string.Empty;
    public DateOnly? Date { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public string Resolution { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class Complaint
{
    public string ComplaintId { get; set; } = string.Empty;
    public DateOnly? Date { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Resolution { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ServiceRequest
{
    public string RequestId { get; set; } = string.Empty;
    public DateOnly? Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
