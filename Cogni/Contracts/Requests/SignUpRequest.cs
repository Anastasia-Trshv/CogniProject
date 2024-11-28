using System.Reflection.Metadata;

namespace Cogni.Contracts.Requests
{
    public record SignUpRequest
    (
        string Name,
        string Email,
        string Password,
        string MbtiType
    );
}
