namespace Cogni.Contracts.Requests
{
    public record PostRequest
    (
        string? PostBody, 
        ICollection<IFormFile>? PostImages
        );
}
