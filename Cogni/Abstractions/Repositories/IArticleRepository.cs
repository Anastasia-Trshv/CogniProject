using Cogni.Database.Entities;
using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface IArticleRepository
    {
        Task<List<ArticleModel>> GetAll();
        Task<ArticleModel> GetById(int id);
        Task<Article> Create(string articleName, string articleBody, List<string> imageUrls, int userId, string annotation, string? articlePreview);
        Task Update(int id, string ArticleName, string articleBody, List<string> imageUrls, string annotation, string? articlePreview);
        Task UpdateReadsNumber(int id, int readsNumber);

        Task Delete(int id);
    }
}