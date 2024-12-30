using RestSharp;
using Newtonsoft.Json.Linq;

namespace Land.Services
{
    public class GoogleMapsService
    {
        private readonly string _apiKey;

        public GoogleMapsService(string apiKey)
        {
            _apiKey = apiKey;
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
