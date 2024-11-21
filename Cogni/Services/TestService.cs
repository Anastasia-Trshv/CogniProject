using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Abstractions.Repositories;

namespace Cogni.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<TestModel> GetAllQuestions()
        {
            var questions = await _testRepository.GetAllQuestions();
            return questions;
        }

        public async Task<QuestionModel?> GetById(int id)
        {
            var question = await _testRepository.GetById(id);
            return question;
        }
    }
}
