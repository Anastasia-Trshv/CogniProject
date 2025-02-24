using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using Cogni.Abstractions.Services;

namespace Cogni.Services
{
    public interface IImageProcessingService
    {
        Task<MemoryStream> ProcessImage(IFormFile file, ImageFormat outputFormat = ImageFormat.Jpeg);
    }

    public enum ImageFormat
    {
        Jpeg,
        Png
    }

    public class ImageProcessingService : IImageProcessingService
    {
        private const int MinDimension = 64;
        private const int MaxDimension = 1024;
        
        public async Task<MemoryStream> ProcessImage(IFormFile file, ImageFormat outputFormat = ImageFormat.Jpeg)
        {
            using var inputStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);
            
            // Определяем необходимость и параметры масштабирования
            var (newWidth, newHeight) = CalculateNewDimensions(image.Width, image.Height);
            
            // Если размеры изменились, масштабируем изображение
            if (newWidth != image.Width || newHeight != image.Height)
            {
                image.Mutate(x => x.Resize(newWidth, newHeight));
            }

            var outputStream = new MemoryStream();
            
            // Сохраняем в выбранном формате
            switch (outputFormat)
            {
                case ImageFormat.Jpeg:
                    await image.SaveAsJpegAsync(outputStream, new JpegEncoder
                    {
                        Quality = 85 // Оптимальный баланс между качеством и размером
                    });
                    break;
                    
                case ImageFormat.Png:
                    await image.SaveAsPngAsync(outputStream, new PngEncoder
                    {
                        CompressionLevel = PngCompressionLevel.BestCompression
                    });
                    break;
                    
                default:
                    throw new ArgumentException("Неподдерживаемый формат изображения");
            }
            
            outputStream.Position = 0;
            return outputStream;
        }

        private (int width, int height) CalculateNewDimensions(int currentWidth, int currentHeight)
        {
            var aspectRatio = (float)currentWidth / currentHeight;

            // Если изображение слишком большое
            if (currentWidth > MaxDimension || currentHeight > MaxDimension)
            {
                if (currentWidth > currentHeight)
                {
                    // Для широких изображений
                    return (MaxDimension, (int)Math.Round(MaxDimension / aspectRatio));
                }
                else
                {
                    // Для высоких изображений
                    return ((int)Math.Round(MaxDimension * aspectRatio), MaxDimension);
                }
            }

            // Если изображение слишком маленькое
            if (currentWidth < MinDimension || currentHeight < MinDimension)
            {
                if (currentWidth < currentHeight)
                {
                    // Для узких изображений
                    return (MinDimension, (int)Math.Round(MinDimension / aspectRatio));
                }
                else
                {
                    // Для широких маленьких изображений
                    return ((int)Math.Round(MinDimension * aspectRatio), MinDimension);
                }
            }

            // Если размеры в допустимых пределах, оставляем как есть
            return (currentWidth, currentHeight);
        }
    }
}