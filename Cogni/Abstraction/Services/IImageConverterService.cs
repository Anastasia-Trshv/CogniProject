using System.IO;

namespace Cogni.Abstraction.Services
{
    public interface IImageConverterService
    {
        IFormFile ConvertAndResizeImage(IFormFile input, string targetFormat = "jpeg");
    }
} 