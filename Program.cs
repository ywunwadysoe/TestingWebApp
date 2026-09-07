using TestingWebApp.Options;
using TestingWebApp.Repositories;

namespace TestingWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<CustomerDatabaseOptions>(
            builder.Configuration.GetSection(CustomerDatabaseOptions.SectionName));

        builder.Services.AddSingleton<CustomerJsonRepository>();
        builder.Services.AddScoped<Services.CustomerService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactFrontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = false;
            });

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseCors("ReactFrontend");
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();
        app.Run();
    }
}
