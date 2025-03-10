using Cogni.Abstractions.Services;
using Cogni.Abstractions.Repositories;
using SixLabors.ImageSharp;
using Cogni.Abstraction.Services;

namespace Cogni.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageConverterService _imageConverterService; 
        public ImageService(IImageRepository imageRepository, IImageConverterService imageConverter)
        {
            _imageRepository = imageRepository;
            _imageConverterService = imageConverter;
        }
        // Добавляет изображение на сервер и возвращает URL с ID
        public async Task<String> UploadImage(IFormFile inputFile)
        {
            var file = _imageConverterService.ConvertAndResizeImage(inputFile);

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

        // Удаляет изображение с сервера по ID
        public async Task DeleteImage(string id)
        {
            await _imageRepository.DeleteImage(id);
        }
    }
}














