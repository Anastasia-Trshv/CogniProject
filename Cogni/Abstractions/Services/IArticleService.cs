using Cogni.Database.Entities;
using Cogni.Models;

namespace Cogni.Abstractions.Services
{
    public interface IArticleService
    {
        Task<List<ArticleModel>> GetAll();
        Task<ArticleModel> GetArticleByIdAsync(int id);
        Task CreateArticle(string articleName, string articleBody, List<string> imageUrls, int userId);
        Task Update(int id, string ArticleName, string articleBody, List<string> imageUrls);
        Task DeleteArticleAsync(int id);
    }
}

