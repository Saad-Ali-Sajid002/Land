using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Land.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly string _apiKey = "AIzaSyAMCuhMSeDuZRdvscIWAwYk9KCrbGJ6hwg";

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
                imageUrls.Add($"https://maps.googleapis.com/maps/api/streetview?size=640x640&location={address}&heading={i * 90}&key={_apiKey}");
            }
            return imageUrls;
        }
    }
}
