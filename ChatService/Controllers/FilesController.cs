using Minio;
using Minio.DataModel.Args;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace ChatService.Controllers;

[Route("Chat/Files")]
public class FilesController : ControllerBase
{
    private readonly IMinioClient _minioClient;
    private const string BucketName = "chat-files";
    public FilesController(IMinioClientFactory minioFactory)
    {
        _minioClient = minioFactory.CreateClient();
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> files)
    {
        if (files.Count > 9)
        {
            return BadRequest("You can upload a maximum of 9 files.");
        }
        const long maxSize = 1L * 1024 * 1024 * 1024;
        var uploadedFiles = new List<string>(); 
        foreach (var file in files)
        {
            if (file == null || file.Length > maxSize || file.Length == 0){continue;}
            var originalFileName = file.FileName;
            var fileExtension = Path.GetExtension(originalFileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var normalizedFileName = $"{fileNameWithoutExtension}{fileExtension.ToLower()}";
            var uniqueFileName = $"{Guid.NewGuid()}_{normalizedFileName}";
            using var fileStream = file.OpenReadStream();
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(uniqueFileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType("application/octet-stream")
                );
            uploadedFiles.Add($"/{BucketName}/{uniqueFileName}");
        }
        return Ok(new {links = uploadedFiles});
    }
}
