namespace Cogni.Contracts.Requests
{
    public record PostRequest
    (
        string? PostBody,
        IFormFileCollection? Files
        );
        
}
