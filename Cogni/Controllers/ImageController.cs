using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Extensions.Logging;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ImageController : ControllerBase
    {

        private readonly IImageService _imageService;

        private readonly ILogger<ImageController> _logger;

        public ImageController(IImageService imageService, ILogger<ImageController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddImage([FromForm] AddImageRequest request)
        {
            // todo: IDENTIFY AND VALIDATE USER!!!!
            try
            {
                var url = await _imageService.UploadImage(request.ImageFile);
                if (url == null)
                {
                    _logger.LogError("Ошибка при загрузке изображения!");
                    return StatusCode(500, "Внутренняя ошибка сервера");
                }
                return Ok(url);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при загрузке изображения: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(DeleteImageRequest request)
        {
            // todo: IDENTIFY AND VALIDATE USER!!!!
            try
            {
                await _imageService.DeleteImage(request.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении изображения: {ex.Message}");
            }
            return Ok();
        }
    }
}