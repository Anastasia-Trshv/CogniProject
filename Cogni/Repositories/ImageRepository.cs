using Cogni.Abstractions.Repositories;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace Cogni.Database.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private string UPLOADCARE_BASE_CDN;
        private string UPLOADCARE_PUB_KEY;
        private string UPLOADCARE_SECRET_KEY;

        private HttpClient _httpClient;

        // Добавляет изображение на сервер и возвращает URL с ID

        public ImageRepository(IConfiguration config, HttpClient httpClient) 
        {
            UPLOADCARE_BASE_CDN = EnsureConfigStringNotNull(config, "ImageRepository:UPLOADCARE_BASE_CDN");
            EnsureUploadcareCdnBaseValid(UPLOADCARE_BASE_CDN);

            UPLOADCARE_PUB_KEY = EnsureConfigStringNotNull(config, "ImageRepository:UPLOADCARE_PUB_KEY");
            if (!Regex.IsMatch(UPLOADCARE_PUB_KEY, @"^[a-z0-9]+$")) throw new InvalidOperationException(
                "Неверный формат 'ImageRepository:UPLOADCARE_PUB_KEY'."
            );

            UPLOADCARE_SECRET_KEY = EnsureConfigStringNotNull(config, "ImageRepository:UPLOADCARE_SECRET_KEY");
            if (!Regex.IsMatch(UPLOADCARE_SECRET_KEY, @"^[a-z0-9]+$")) throw new InvalidOperationException(
                "Неверный формат 'ImageRepository:UPLOADCARE_SECRET_KEY'."
            );

            _httpClient = httpClient;
        }

        private static void EnsureUploadcareCdnBaseValid(string url)
        {
            string pattern = @"^https:\/\/[a-zA-Z0-9-]+\.ucarecd\.net$";
            if (!Regex.IsMatch(url, pattern))
                throw new InvalidOperationException($"Неверный базовый uploadcare CDN URL: `{url}`");
        }

        private static string EnsureConfigStringNotNull(IConfiguration config, string field)
        {
            var str = config[field]?.Trim() ?? string.Empty;
            if (String.IsNullOrWhiteSpace(str))
                throw new InvalidOperationException($"Не задан параметр конфигурации {field}");
            return str;
        }

        public async Task<String> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл не выбран или пуст");
            
            using var formData = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            using var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
                file.ContentType ?? "application/octet-stream"
            );

            formData.Add(fileContent, "file", file.FileName);
            formData.Add(new StringContent(UPLOADCARE_PUB_KEY), "UPLOADCARE_PUB_KEY");

            var response = await _httpClient.PostAsync(
                "https://upload.uploadcare.com/base/",
                formData
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Ошибка при загрузке изображения");
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseContent);
            if (!jsonDoc.RootElement.TryGetProperty("file", out var fileIdEl))
                throw new Exception("Ответ от сервера не содержит ID файла");
            
            return $"{UPLOADCARE_BASE_CDN}/{fileIdEl.GetString()}/";
        }
        
        // Удаляет изображение с сервера по ID
        public async Task DeleteImage(string fileId)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Uploadcare.Simple {UPLOADCARE_PUB_KEY}:{UPLOADCARE_SECRET_KEY}");
            var response = await _httpClient.DeleteAsync($"https://api.uploadcare.com/files/{fileId}/");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Ошибка при удалении изображения");
        }
    }
}
