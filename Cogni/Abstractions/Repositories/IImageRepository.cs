using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface IImageRepository
    {
        Task<ImageUrlModel> UploadImage(IFormFile file);
        Task DeleteImage(string fileId);
    }
}
