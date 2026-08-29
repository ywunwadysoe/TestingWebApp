namespace TestingWebApp.Models;

public class CustomerBehaviour
{
    public DateTimeOffset? LastLogin { get; set; }
    public int LoginCountLast30Days { get; set; }
    public List<string> FrequentlyViewedServices { get; set; } = [];
    public string LastViewedPolicy { get; set; } = string.Empty;
    public string PreferredContactTime { get; set; } = string.Empty;
    public List<string> PreferredChannels { get; set; } = [];
    public List<string> CommonQuestions { get; set; } = [];
}
