using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace Land.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly string _apiKey;
        private readonly IHttpClientFactory _httpClientFactory;

        // Mark Address as nullable if it can be null
        public string? Address { get; set; } = "Unknown Address"; // Default value
        public List<string> DistressNotes { get; set; }
        public List<string> ImageUrls { get; set; }

        public DetailsModel(IHttpClientFactory httpClientFactory, IOptions<GoogleApiSettings> googleApiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = googleApiSettings.Value.ApiKey ?? throw new ArgumentNullException("Google API Key is missing.");
            DistressNotes = new List<string>(); 
            ImageUrls = new List<string>(); 
        }

        public async Task OnGetAsync(string address, string distressNotes)
        {
            Address = address ?? "Unknown Address"; // Default value
            DistressNotes = string.IsNullOrEmpty(distressNotes) ? new List<string>() : distressNotes.Split(";").ToList();

            try
            {
                ImageUrls = await GetStreetViewImagesAsync(address);  // Fetch multiple images based on the address
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ImageUrls = new List<string>(); 
            }
        }

        private async Task<List<string>> GetStreetViewImagesAsync(string address)
        {
            var imageUrls = new List<string>();

            var encodedAddress = Uri.EscapeDataString(address);

            var client = _httpClientFactory.CreateClient();

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    var imageUrl = $"https://maps.googleapis.com/maps/api/streetview?size=640x640&location={encodedAddress}&heading={i * 90}&key={_apiKey}";
                    imageUrls.Add(imageUrl);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching street view images: {ex.Message}");
            }

            return imageUrls;
        }
    }

    public class GoogleApiSettings
    {
        public string ApiKey { get; set; } 
    }
}
