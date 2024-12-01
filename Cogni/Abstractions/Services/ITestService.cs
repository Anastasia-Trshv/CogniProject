using Cogni.Models;
using Cogni.Database.Entities;

namespace Cogni.Abstractions.Services
{
    public interface ITestService
    {
        Task<TestModel> GetAllQuestions();
        Task<QuestionModel?> GetById(int id);
    }
}
