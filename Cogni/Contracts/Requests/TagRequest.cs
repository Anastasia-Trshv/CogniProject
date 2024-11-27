using Microsoft.AspNetCore.Mvc;

namespace Cogni.Contracts.Requests
{
    public record TagRequest 
    (
        int Id ,
        string? NameTag 
    );
}
