var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register IHttpClientFactory
builder.Services.AddHttpClient();

// Add environment variables for the API key
builder.Services.Configure<GoogleApiSettings>(builder.Configuration.GetSection("GoogleApi"));

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
