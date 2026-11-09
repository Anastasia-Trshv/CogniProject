using Microsoft.AspNetCore.Mvc;
using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Contracts.Requests;
using Cogni.Services;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly SakinorvaService _sakinorvaService;

        public TestController(ITestService testService, SakinorvaService sakinorvaService)
        {
            _testService = testService;
            _sakinorvaService = sakinorvaService;
        }
        /// <summary>
        /// Получение всех вопросов теста
        /// </summary>
        /// <response code="200">Вопросы отправлены</response>
        /// <response code="404">В бд нет вопросов или что-то сломалось</response>
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions(CancellationToken cancellationToken)
        {
            // Старый URL теперь возвращает вопросы нового MBTI-теста.
            using var catalog = await _sakinorvaService.GetCatalogAsync(cancellationToken);
            var root = catalog.RootElement;
            var questions = root.GetProperty("questions").EnumerateArray().Select((q, index) => new
            {
                idMbtiQuestion = index + 1,
                id = q.GetProperty("id").GetString(),
                function = q.TryGetProperty("function", out var function) ? function.GetString() : null,
                question = q.GetProperty("question").GetString(),
                example = q.TryGetProperty("example", out var example) ? example.GetString() : null,
                isControl = !q.TryGetProperty("function", out _)
            }).ToArray();
            return Ok(new { questions, scale = root.GetProperty("scale").Clone() });
        }

        /// <summary>Подсчёт результата нового теста через прежний контроллер Test.</summary>
        [HttpPost]
        public async Task<IActionResult> CalculateResult([FromBody] CalculateSakinorvaRequest request, CancellationToken cancellationToken)
        {
            if (request.Answers is null) return BadRequest(new { error = "Поле answers обязательно." });
            try
            {
                return Ok(await _sakinorvaService.CalculateAsync(request.Answers, cancellationToken));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Получение вопроса теста по id
        /// </summary>
        /// <response code="200">Вопросы отправлен</response>
        /// <response code="404">В бд нет вопросов или что-то сломалось</response>
        [HttpGet]
        public async Task<IActionResult> GetQuestion(QuestionRequest request)
        {
            var question = await _testService.GetById(request.id);
            if (question != null)
            {
                return Ok(question);
            }
            return NotFound();
        }
    }
}