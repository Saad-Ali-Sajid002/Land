using RestSharp;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;

namespace Land.Services
{
    public class GoogleMapsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public GoogleMapsService(IHttpClientFactory httpClientFactory, string apiKey)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
        }

        public async Task<(double, double)> GetCoordinatesAsync(string address)
        {
            var client = _httpClientFactory.CreateClient();  // Using IHttpClientFactory for HTTP client creation
            var request = new RestRequest($"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}");
            
            // Execute the request asynchronously
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var json = JObject.Parse(response.Content);
                var location = json["results"]?[0]?["geometry"]?["location"];
                if (location != null)
                {
                    double lat = location["lat"].Value<double>();
                    double lng = location["lng"].Value<double>();
                    return (lat, lng);
                }
            }
            throw new Exception("Failed to fetch coordinates.");
        }
    }
}
