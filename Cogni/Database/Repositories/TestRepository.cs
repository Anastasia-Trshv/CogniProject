using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Models;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Repositories
{
    public class TestRepository : ITestRepository
    {
        readonly CogniDbContext _context;
        public TestRepository(CogniDbContext context)
        {
            _context = context;
        }

        public async Task<TestModel> GetAllQuestions()
        {
            var questions = await _context.MbtiQuestions.ToListAsync();
            var questionModels = questions.Select(q => 
                new QuestionModel(q.IdMbtiQuestion, q.Question)).ToList();
            return new TestModel(questionModels);
        }

        public async Task<QuestionModel?> GetById(int id)
        {
            var question = await _context.MbtiQuestions
                .FirstOrDefaultAsync(u => u.IdMbtiQuestion == id);
            return question == null ? null : 
                new QuestionModel(question.IdMbtiQuestion, question.Question);
        }

        public async Task SetTestResult(UserModel user, int mbtiId)
        {
            var u = await _context.Customusers
                .FirstOrDefaultAsync(u => u.IdUser == user.IdUser);
            if (u != null)
            {
                u.IdMbtiType = mbtiId;
                await _context.SaveChangesAsync();
                return;
            }
            // ачо делать если нет юзера?
            // todo: handle error
            return;
        }
    }
}
