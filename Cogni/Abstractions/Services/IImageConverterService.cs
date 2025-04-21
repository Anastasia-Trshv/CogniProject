using System.IO;

namespace Cogni.Abstractions.Services;
public interface IImageConverterService
{
    IFormFile ConvertAndResizeImage(IFormFile input, string targetFormat = "jpeg");
}
