using Microsoft.AspNetCore.Mvc;
using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Contracts.Requests;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        /// <summary>
        /// Получение всех вопросов теста
        /// </summary>
        /// <response code="200">Вопросы отправлены</response>
        /// <response code="404">В бд нет вопросов или что-то сломалось</response>
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _testService.GetAllQuestions();
            if (questions == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(questions);
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