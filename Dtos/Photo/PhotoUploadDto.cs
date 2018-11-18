using Microsoft.AspNetCore.Http;

namespace PremierLeagueAPI.Dtos.Photo
{
    public class PhotoUploadDto
    {
        public IFormFile File { get; set; }
    }
}