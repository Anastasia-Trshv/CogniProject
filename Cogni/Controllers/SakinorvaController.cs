using Cogni.Contracts.Requests;
using Cogni.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cogni.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class SakinorvaController : ControllerBase
{
    private readonly SakinorvaService _service;
    public SakinorvaController(SakinorvaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetQuestions(CancellationToken cancellationToken)
    {
        using var catalog = await _service.GetCatalogAsync(cancellationToken);
        return Ok(catalog.RootElement.Clone());
    }

    [HttpPost]
    public async Task<IActionResult> Calculate([FromBody] CalculateSakinorvaRequest request, CancellationToken cancellationToken)
    {
        if (request.Answers is null) return BadRequest(new { error = "Поле answers обязательно." });
        try
        {
            return Ok(await _service.CalculateAsync(request.Answers, cancellationToken));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
