﻿using Cogni.Database.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Contracts.Requests;

namespace Cogni.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IImageService _imageService;

        public ArticleService(IArticleRepository articleRepository, IImageService imageService)
        {
            _articleRepository = articleRepository;
            _imageService = imageService;
        }

        public async Task<List<ArticleModel>> GetAllAsync()
        {
            var articles = await _articleRepository.GetAll();
            return articles.Select(article => ToModel(article)).ToList();
        }

        public async Task<List<(int Id, string? ArticleName)>> GetAllArticleIdsAndNamesAsync()
        {
            var articles = await _articleRepository.GetAll();
            return articles.Select(article => (article.Id, article.ArticleName)).ToList();
        }

        public async Task<ArticleModel> GetArticleByIdAsync(int id)
        {
            var article = await _articleRepository.GetById(id);
            return article != null ? ToModel(article) : null;
        }

        public async Task<Article> CreateArticleAsync(CreateArticleRequest request, int userId)
        {
            var article = new Article
            {
                ArticleName = request.ArticleName,
                ArticleBody = request.ArticleBody,
                IdUser = userId,
                ArticleImages = new List<ArticleImage>()
            };

            //TODO: Upload images to cloud and get URLs
            if (request.Files != null)
            {
                foreach (var file in request.Files)
                {
                    var imageUrl = await _imageService.UploadImage(file);
                    article.ArticleImages.Add(new ArticleImage { ImageUrl = imageUrl });
                }
            }

            List<string> imageUrls = article.ArticleImages.Select(ai => ai.ImageUrl).ToList();

            Article createdArticle = await _articleRepository.Create(article.ArticleName, article.ArticleBody, imageUrls, article.IdUser);

            return createdArticle;
        }



        public async Task<ArticleModel> UpdateArticleAsync(int id, string articleName, string articleBody, IFormFileCollection files, int userId)
        {
            var article = await _articleRepository.GetById(id);

            if (article == null)
            {
                throw new Exception("Статья не найдена.");
            }

            // Обновляем название и содержимое статьи
            article.ArticleName = articleName;
            article.ArticleBody = articleBody;

            // Получаем существующие URL изображений
            List<string> imageUrls = article.ArticleImages.Select(ai => ai.ImageUrl).ToList();

            // Обрабатываем загруженные изображения
            if (files != null)
            {
                foreach (var file in files)
                {
                    var imageUrl = await _imageService.UploadImage(file);
                    imageUrls.Add(imageUrl);
                    article.ArticleImages.Add(new ArticleImageModel { ImageUrl = imageUrl });
                }
            }

            // Обновляем статью в репозитории
            await _articleRepository.Update(article.Id, article.ArticleName, article.ArticleBody, imageUrls);

            // Возвращаем обновленную статью
            return article;
        }





        public async Task DeleteArticleAsync(int id)
        {
            await _articleRepository.Delete(id);
        }
        private ArticleModel ToModel(ArticleModel article)
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
