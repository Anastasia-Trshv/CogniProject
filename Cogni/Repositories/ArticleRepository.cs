using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Database.Entities;
using Cogni.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cogni.Database.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly CogniDbContext _context;

        public ArticleRepository(CogniDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArticleModel>> GetAll()
        {
            var articles = await _context.Articles
                .Include(a => a.ArticleImages)
                .ToListAsync();

            return articles.Select(a => ToModel(a)).ToList();
        }

        public async Task<ArticleModel?> GetById(int id)
        {
            var article = await _context.Articles
                .Include(a => a.ArticleImages)
                .FirstOrDefaultAsync(a => a.Id == id);

            return article != null ? ToModel(article) : null;
        }

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

        public async Task Update(int id, string articleName, string articleBody, List<string> imageUrls)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                article.ArticleName = articleName;
                article.ArticleBody = articleBody;

                article.ArticleImages.Clear();
                foreach (var imageUrl in imageUrls)
                {
                    article.ArticleImages.Add(new ArticleImage { ImageUrl = imageUrl });
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        { 
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);


                await _context.SaveChangesAsync();
            }
        }


        public async Task<Article> Create(string articleName, string articleBody, List<string> imageUrls, int userId)
        {
            var article = new Article
            {
                ArticleName = articleName,
                ArticleBody = articleBody,
                IdUser = userId
            };

            foreach (var imageUrl in imageUrls)
            {
                article.ArticleImages.Add(new ArticleImage { ImageUrl = imageUrl });
            }

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return article;
        }
    }
}