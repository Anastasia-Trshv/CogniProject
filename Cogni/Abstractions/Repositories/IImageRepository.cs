using Cogni.Models;
using Microsoft.AspNetCore.Http;

namespace Cogni.Abstractions.Repositories
{
    public interface IImageRepository
    {
        Task<String> UploadImage(IFormFile file); //добавляет изображение на сервер и возвращает URL с ID
        Task DeleteImage(string fileId); //удаляет изображение с сервера
    }
}
