using Cogni.Models;
using Cogni.Database.Entities;
using Microsoft.AspNetCore.Http;

namespace Cogni.Abstractions.Services
{
    public interface IImageService
    {
        Task<ImageUrlModel> UploadImage(IFormFile file);
        Task DeleteImage(string id);
    }
}
