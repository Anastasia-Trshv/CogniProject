using Cogni.Contracts.Requests;
using Cogni.Database.Entities;
using Cogni.Models;

public interface IArticleService
{
    Task<List<ArticleModel>> GetAllAsync();
    Task<ArticleModel> GetArticleByIdAsync(int id);
    Task<Article> CreateArticleAsync(CreateArticleRequest request, int userId);

    Task<ArticleModel> UpdateArticleAsync(int id, string articleName, string articleBody, IFormFileCollection files, int userId);

    Task DeleteArticleAsync(int id);
}


