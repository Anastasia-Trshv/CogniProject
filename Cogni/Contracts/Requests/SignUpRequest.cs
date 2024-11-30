using System.Reflection.Metadata;

namespace Cogni.Contracts.Requests
{
    public record SignUpRequest
    (
        string Name,
        string Surname,
        string Email,
        string Password,
        string MbtiType
    );
}
