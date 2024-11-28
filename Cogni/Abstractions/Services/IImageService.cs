using Cogni.Models;
using Cogni.Database.Entities;
using Microsoft.AspNetCore.Http;

namespace Cogni.Abstractions.Services
{
    public interface IImageService
    {
        Task<String> UploadImage(IFormFile file); //добавляет изображение на сервер и возвращает URL с ID
        Task DeleteImage(string id); //удаляет изображение с сервера по ID
    }
}
