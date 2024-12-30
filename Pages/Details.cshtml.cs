using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Land.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly string _apiKey;

        public DetailsModel()
        {
            // Retrieve the API key from environment variables
            _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");

            // Fallback to static API key for local testing if the environment variable is not set
            if (string.IsNullOrEmpty(_apiKey))
            {
                // Static API key for local testing
                _apiKey = "AIzaSyAMCuhMSeDuZRdvscIWAwYk9KCrbGJ6hwg";
                Console.WriteLine("Environment variable not set. Using static API key for local testing.");
            }

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("The API key is not available. Please set GOOGLE_API_KEY environment variable or configure the static key.");
            }
        }

        public string Address { get; set; }
        public List<string> DistressNotes { get; set; }
        public List<string> ImageUrls { get; set; }  // Changed to a list of image URLs

        public async Task OnGetAsync(string address, string distressNotes)
        {
            Address = address;
            DistressNotes = distressNotes?.Split(";").ToList() ?? new List<string>();
            ImageUrls = await GetStreetViewImagesAsync(address);  // Fetch multiple images
        }

        private async Task<List<string>> GetStreetViewImagesAsync(string address)
        {
            // Fetch multiple images (e.g., different directions or views) for the selected address
            var imageUrls = new List<string>();
            for (int i = 0; i < 3; i++)  // Example: fetch 3 different images
            {
                imageUrls.Add($"https://maps.googleapis.com/maps/api/streetview?size=640x640&location={Uri.EscapeDataString(address)}&heading={i * 90}&key={_apiKey}");
            }
            return imageUrls;
        }
    }
}
