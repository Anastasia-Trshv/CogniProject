using Cogni.Database.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Models;

namespace Cogni.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<List<ArticleModel>> GetAll()
        {
            var articles = await _articleRepository.GetAll();
            return articles.Select(ToModel).ToList();
        }

        public async Task<ArticleModel> GetArticleByIdAsync(int id)
        {
            var article = await _articleRepository.GetById(id);
            return article != null ? ToModel(article) : null;
        }

        public async Task CreateArticle(string articleName, string articleBody, List<string> imageUrls, int userId)
        {
            await _articleRepository.Create(articleName, articleBody, imageUrls, userId);
        }

        private ArticleModel ToModel(ArticleModel article)
        {
            throw new NotImplementedException();
        }

        public async Task Update(int id, string ArticleName, string articleBody, List<string> imageUrls)
        {
            await _articleRepository.Update(id, ArticleName, articleBody, imageUrls);
        }

        public async Task DeleteArticleAsync(int id) => await _articleRepository.Delete(id);

        private ArticleModel ToModel(Article article)
        {
            return new ArticleModel
            {
                Id = article.Id,
                ArticleName = article.ArticleName,
                ArticleBody = article.ArticleBody,
                IdUser = article.IdUser,
                ArticleImages = article.ArticleImages.Select(ai => new ArticleImageModel
                {
                    Id = ai.Id,
                    ArticleId = ai.ArticleId,
                    ImageUrl = ai.ImageUrl
                }).ToList(),
                IdUserNavigation = article.IdUserNavigation
            };
        }
    }
}