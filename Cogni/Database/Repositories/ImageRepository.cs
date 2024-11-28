using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace Cogni.Database.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private const string UPLOADCARE_PUB_KEY = "3aff0939747d03d6b4f4";
        private const string UPLOADCARE_SECRET_KEY = "86eebd9da7f3bc584505";

        private readonly ILogger<ImageRepository> _logger;
        public ImageRepository(ILogger<ImageRepository> logger)
        {
            _logger = logger;
        }

        public async Task<ImageUrlModel> UploadImage(IFormFile file)
        {
            using var client = new HttpClient();
            using var formData = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenReadStream());
            formData.Add(fileContent, "file", file.FileName);
            formData.Add(new StringContent(UPLOADCARE_PUB_KEY), "UPLOADCARE_PUB_KEY");
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            var response = await client.PostAsync(
                "https://upload.uploadcare.com/base/",
                formData
            );
            if (!response.IsSuccessStatusCode)
                throw new Exception("Ошибка при загрузке изображения");
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var fileId = JsonDocument.Parse(responseContent).RootElement.GetProperty("file").GetString();
            return new ImageUrlModel{ Url = $"https://ucarecdn.com/{fileId}/" };
        }

        public async Task DeleteImage(string fileId)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Uploadcare.Simple {UPLOADCARE_PUB_KEY}:{UPLOADCARE_SECRET_KEY}");
            var response = await client.DeleteAsync($"https://api.uploadcare.com/files/{fileId}/");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Ошибка при удалении изображения");
        }
    }
}
