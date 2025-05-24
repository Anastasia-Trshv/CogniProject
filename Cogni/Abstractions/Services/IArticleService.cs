using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Cogni.Database.Entities;
using Cogni.Models;

public interface IArticleService
{
    Task<List<ArticleModel>> GetAllAsync();
    Task<ArticleModel> GetArticleByIdAsync(int id);
    Task<Article> CreateArticleAsync(CreateArticleRequest request, int userId);

    Task<ArticleModel> UpdateArticleAsync(int id, ArticleUpdateRequest request, int userId);
    Task IncrementArticleReadsAsync(int id);
    Task<ArticlePreviewResponse?[]> GetArticlesPreviewesAsync();

    Task DeleteArticleAsync(int id);
}