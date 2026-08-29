namespace TestingWebApp.Options;

public class CustomerDatabaseOptions
{
    public const string SectionName = "CustomerDatabase";

    public string FilePath { get; set; } = "Data/customers.json";
}
