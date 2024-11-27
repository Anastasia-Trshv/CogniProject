namespace Cogni.Contracts.Requests
{
    public record PostRequest
    (
        int Id,
        string? PostBody, 
        int IdUser,
        ICollection<byte[]>? PostImages
        );
}
