using Cogni.Abstractions.Repositories;
using System.Text.Json;
namespace Cogni.Database.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private string UPLOADCARE_PUB_KEY;
        private string UPLOADCARE_SECRET_KEY;

        // Добавляет изображение на сервер и возвращает URL с ID

        public ImageRepository(IConfiguration config) 
        {
            UPLOADCARE_PUB_KEY = config["ImageRepository:UPLOADCARE_PUB_KEY"];
            UPLOADCARE_SECRET_KEY = config["ImageRepository:UPLOADCARE_SECRET_KEY"];
        }
        public async Task<String> UploadImage(IFormFile file)
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
            return $"https://ucarecdn.com/{fileId}/";
        }
        // Удаляет изображение с сервера по ID
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
