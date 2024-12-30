using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register IHttpClientFactory
builder.Services.AddHttpClient();

// Configure GoogleApiSettings to read from environment variables (Render or Azure inject these at runtime)
builder.Services.Configure<GoogleApiSettings>(options =>
{
    // First check for the environment variable directly
    options.ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ??
                     builder.Configuration.GetValue<string>("GoogleApi:ApiKey");

    if (string.IsNullOrEmpty(options.ApiKey))
    {
        throw new ArgumentException("Google API Key is missing. Please set GOOGLE_API_KEY environment variable.");
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

public class GoogleApiSettings
{
    public string ApiKey { get; set; }
}
