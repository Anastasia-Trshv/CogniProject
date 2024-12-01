using Cogni.Database.Entities;
using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface IArticleRepository
    {
        Task<List<ArticleModel>> GetAll();
        Task<ArticleModel> GetById(int id);
        Task Update(int id, string ArticleName, string articleBody, List<string> imageUrls);
        Task Delete(int id);
    }
}