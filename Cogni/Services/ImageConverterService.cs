using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Cogni.Abstractions.Services;

namespace Cogni.Services;

public class ImageConverterService : IImageConverterService
{
    private const int MinSize = 64;
    private const int MaxSize = 1024;

    public IFormFile ConvertAndResizeImage(IFormFile inputFile, string targetFormat = "jpeg")
    {
        if (inputFile == null)
            throw new ArgumentNullException(nameof(inputFile));

        // Проверка допустимого значения targetFormat
        if (targetFormat != "jpeg" && targetFormat != "png")
            throw new ArgumentException("Допустимые форматы: 'jpeg' или 'png'", nameof(targetFormat));

        try
        {
            using (var image = Image.Load(inputFile.OpenReadStream()))
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                double scaleFactor = 1.0;

                // Если изображение больше допустимого, вычисляем коэффициент уменьшения
                if (originalWidth > MaxSize || originalHeight > MaxSize)
                {
                    double ratioWidth = (double)MaxSize / originalWidth;
                    double ratioHeight = (double)MaxSize / originalHeight;
                    scaleFactor = Math.Min(ratioWidth, ratioHeight);
                }
                // Если изображение меньше допустимого, масштабируем вверх
                else if (originalWidth < MinSize || originalHeight < MinSize)
                {
                    double ratioWidth = (double)MinSize / originalWidth;
                    double ratioHeight = (double)MinSize / originalHeight;
                    scaleFactor = Math.Max(ratioWidth, ratioHeight);
                }

                // Если масштабирование требуется, изменяем размер с сохранением пропорций
                if (Math.Abs(scaleFactor - 1.0) > 0.01)
                {
                    int newWidth = (int)(originalWidth * scaleFactor);
                    int newHeight = (int)(originalHeight * scaleFactor);
                    image.Mutate(ctx => ctx.Resize(newWidth, newHeight));
                }

                // Создаем поток для сохранения результата
                var outputStream = new MemoryStream();

                // Сохраняем в нужном формате
                if (targetFormat == "jpeg")
                {
                    image.SaveAsJpeg(outputStream, new JpegEncoder
                    {
                        Quality = 90 // Хорошее качество, но с разумным сжатием
                    });
                }
                else // targetFormat == "png"
                {
                    image.SaveAsPng(outputStream, new PngEncoder
                    {
                        CompressionLevel = PngCompressionLevel.DefaultCompression // Средний уровень сжатия
                    });
                }

                // Создаем объект FormFile
                outputStream.Position = 0; // Сбрасываем позицию потока
                var outputFile = new FormFile(outputStream, 0, outputStream.Length, inputFile.Name, $"{Path.GetFileNameWithoutExtension(inputFile.FileName)}.{targetFormat}")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = targetFormat == "jpeg" ? "image/jpeg" : "image/png"
                };

                return outputFile;
            }
        }
        catch (UnknownImageFormatException ex)
        {
            throw new ArgumentException("Неподдерживаемый формат изображения", nameof(inputFile), ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Ошибка при обработке изображения", ex);
        }
    }
}
