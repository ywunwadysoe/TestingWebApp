namespace TestingWebApp.Models;

public class Notification
{
    public string NotificationId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string PolicyId { get; set; } = string.Empty;
    public DateTimeOffset? SentDate { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
