using RestSharp;
using Newtonsoft.Json.Linq;

namespace Land.Services
{
    public class GoogleMapsService
    {
        private readonly string _apiKey;

        public GoogleMapsService()
        {

           _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            if (string.IsNullOrEmpty(_apiKey))
            {
                _apiKey = "AIzaSyAMCuhMSeDuZRdvscIWAwYk9KCrbGJ6hwg";
            }

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("The API key is not available. Please set GOOGLE_API_KEY environment variable or configure the static key.");
            }
        }

        public async Task<(double, double)> GetCoordinatesAsync(string address)
        {
            var client = new RestClient($"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}");
            var response = await client.ExecuteAsync(new RestRequest());

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
