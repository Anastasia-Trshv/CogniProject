using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface ITestRepository
    {
        Task<TestModel> GetAllQuestions();
        Task<QuestionModel?> GetById(int id);
    }
}
