using System.IO;

namespace Cogni.Abstraction.Services
{
    public interface IImageConverterService
    {
        Stream ConvertAndResizeImage(Stream input, string targetFormat = "jpeg");
    }
} 