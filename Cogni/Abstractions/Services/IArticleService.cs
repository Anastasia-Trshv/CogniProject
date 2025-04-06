using Cogni.Contracts.Requests;
using Cogni.Database.Entities;
using Cogni.Models;

public interface IArticleService
{
    Task<List<ArticleModel>> GetAllAsync();
    Task<ArticleModel> GetArticleByIdAsync(int id);
    Task<Article> CreateArticleAsync(CreateArticleRequest request, int userId);

    Task<ArticleModel> UpdateArticleAsync(int id, ArticleUpdateRequest request, int userId);
    Task IncrementArticleReadsAsync(int id);

    Task DeleteArticleAsync(int id);
}


