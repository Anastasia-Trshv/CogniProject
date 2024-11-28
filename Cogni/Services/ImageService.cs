using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Abstractions.Repositories;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Cogni.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<ImageService> _logger;
        public ImageService(IImageRepository imageRepository, ILogger<ImageService> logger)
        {
            _imageRepository = imageRepository;
            _logger = logger;
        }

        public async Task<ImageUrlModel> UploadImage(IFormFile file)
        {
            if (!file.ContentType.StartsWith("image/jpeg") && !file.ContentType.StartsWith("image/png"))
                throw new InvalidDataException("Изображения должны быть в формате JPG или PNG");
            using var stream = file.OpenReadStream();
            using var image = await Image.LoadAsync(stream);
            if (image.Width < 64 || image.Height < 64 || image.Width > 1024 || image.Height > 1024)
            {
                throw new InvalidDataException("Разрешение изображения должно быть между 64x64 и 1024x1024");
            }
            var imageUrl = await _imageRepository.UploadImage(file);
            return imageUrl;
        }

        public async Task DeleteImage(string id)
        {
            await _imageRepository.DeleteImage(id);
        }
    }
}














