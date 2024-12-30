using RestSharp;  // Ensure RestSharp is imported
using Newtonsoft.Json.Linq;  // Ensure Newtonsoft.Json is imported
using System;
using System.Threading.Tasks;

public class GoogleMapsService
{
    private readonly string _apiKey;

    // Constructor
    public GoogleMapsService()
    {
        _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") 
                  ?? throw new Exception("API Key is missing."); // Fetch the API key from environment variable
    }

    public async Task<(double lat, double lng)> GetCoordinatesAsync(string address)
    {
        if (string.IsNullOrEmpty(address)) 
            throw new ArgumentException("Address cannot be null or empty.", nameof(address)); 

        // Initialize RestClient
        var client = new RestClient();  // Use RestClient, not HttpClient
        var request = new RestRequest($"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}", Method.Get);

        try
        {
            // Execute the request asynchronously
            var response = await client.ExecuteAsync(request); // RestSharp's ExecuteAsync

            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            {
                var json = JObject.Parse(response.Content); // Parse the response content
                var location = json["results"]?[0]?["geometry"]?["location"];
                if (location != null)
                {
                    double lat = location["lat"].Value<double>();
                    double lng = location["lng"].Value<double>();
                    return (lat, lng);
                }
                else
                {
                    throw new Exception("Location not found in response.");
                }
            }
            else
            {
                throw new Exception($"API call failed with status code: {response.StatusCode}, and message: {response.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            // You can log the exception details here or rethrow
            throw new Exception($"Error fetching coordinates for address '{address}': {ex.Message}", ex);
        }
    }
}
