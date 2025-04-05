using Minio;
using Minio.DataModel.Args;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace ChatService.Controllers;

[Route("Files")]
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
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            using var fileStream = file.OpenReadStream();
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType("application/octet-stream")
                );
            uploadedFiles.Add($"/{BucketName}/{fileName}");
        }
        return Ok(new {links = uploadedFiles});
    }
}
