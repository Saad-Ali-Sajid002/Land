using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Land.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly string _apiKey;
        private readonly IHttpClientFactory _httpClientFactory;

        public string Address { get; set; }
        public List<string> DistressNotes { get; set; }
        public List<string> ImageUrls { get; set; }  // Changed to a list of image URLs

        // Inject IHttpClientFactory and GoogleApiSettings for API Key retrieval
        public DetailsModel(IHttpClientFactory httpClientFactory, IOptions<GoogleApiSettings> googleApiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = googleApiSettings.Value.ApiKey ?? throw new System.Exception("Google API Key is missing.");
        }

        public async Task OnGetAsync(string address, string distressNotes)
        {
            Address = address;
            DistressNotes = distressNotes?.Split(";").ToList() ?? new List<string>();
            ImageUrls = await GetStreetViewImagesAsync(address);  // Fetch multiple images
        }

        private async Task<List<string>> GetStreetViewImagesAsync(string address)
        {
            var imageUrls = new List<string>();

            // Use the HttpClient to fetch street view images from the Google API
            var client = _httpClientFactory.CreateClient();

            // Example: Fetch 3 different images (in different directions)
            for (int i = 0; i < 3; i++)
            {
                var imageUrl = $"https://maps.googleapis.com/maps/api/streetview?size=640x640&location={address}&heading={i * 90}&key={_apiKey}";
                imageUrls.Add(imageUrl);
            }

            return imageUrls;
        }
    }
}
