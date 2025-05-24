using Cogni.Database.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Humanizer;

namespace Cogni.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;

        public ArticleService(IArticleRepository articleRepository, IImageService imageService, IUserService userService)
        {
            _articleRepository = articleRepository;
            _imageService = imageService;
            _userService = userService;
        }

        public async Task<ArticlePreviewResponse?[]> GetArticlesPreviewesAsync()
        {
            var articles = await _articleRepository.GetAll();

            ArticlePreviewResponse[] articlePreviewResponses = new ArticlePreviewResponse[articles.Count];

            foreach (var article in articles)
            {
                if (article == null)
                {
                    return null;
                }

                var user = await _userService.GetPublicUser(article.IdUser);

                if (user == null)
                {
                    return null;
                }

                // Вычисление времени с момента публикации
                var timeSincePublication = article.Created.HasValue
                    ? article.Created.Value.ToLocalTime().Humanize(culture: new System.Globalization.CultureInfo("ru-RU"))
                    : "Неизвестно";

                articlePreviewResponses.Append(
                    new ArticlePreviewResponse(
                    article.ArticlePreview,
                    user.Image,
                    user.Name,
                    article.ReadsNumber ?? 0,
                    timeSincePublication,
                    user.TypeMbti,
                    article.ArticleName,
                    article.Annotation
                ));
            }
            return articlePreviewResponses;
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
            string? articlePreviewUrl = null;

            if (request.ArticlePreviewFile != null)
            {
                articlePreviewUrl = await _imageService.UploadImage(request.ArticlePreviewFile);
            }
            var article = new Article
            {
                ArticleName = request.ArticleName,
                ArticleBody = request.ArticleBody,
                IdUser = userId,
                Annotation = request.Annotation,
                ArticleImages = new List<ArticleImage>(),
                ArticlePreview = articlePreviewUrl
            };

            if (request.Files != null)
            {
                foreach (var file in request.Files)
                {
                    var imageUrl = await _imageService.UploadImage(file);
                    article.ArticleImages.Add(new ArticleImage { ImageUrl = imageUrl });
                }
            }

            List<string> imageUrls = article.ArticleImages.Select(ai => ai.ImageUrl).ToList();

            Article createdArticle = await _articleRepository.Create(article.ArticleName, article.ArticleBody, imageUrls, article.IdUser, article.Annotation, article.ArticlePreview);

            return createdArticle;
        }



        public async Task<ArticleModel> UpdateArticleAsync(int id, ArticleUpdateRequest request, int userId)
        {
            var article = await _articleRepository.GetById(id);


            if (article == null)
            {
                throw new Exception("Статья не найдена.");
            }

            string? articlePreviewUrl = article.ArticlePreview;
            if (request.ArticlePreviewFile != null)
            {
                articlePreviewUrl = await _imageService.UploadImage(request.ArticlePreviewFile);
            }

            // Обновляем название и содержимое статьи
            article.ArticleName = request.ArticleName;
            article.ArticleBody = request.ArticleBody;
            article.Annotation = request.Annotation;
            article.ArticlePreview = articlePreviewUrl;

            // Получаем существующие URL изображений
            List<string> imageUrls = article.ArticleImages.Select(ai => ai.ImageUrl).ToList();

            // Обрабатываем загруженные изображения
            if (request.Files != null)
            {
                foreach (var file in request.Files)
                {
                    var imageUrl = await _imageService.UploadImage(file);
                    imageUrls.Add(imageUrl);
                    article.ArticleImages.Add(new ArticleImageModel { ImageUrl = imageUrl });
                }
            }

            // Обновляем статью в репозитории
            await _articleRepository.Update(article.Id, article.ArticleName, article.ArticleBody, imageUrls, request.Annotation, article.ArticlePreview);

            // Возвращаем обновленную статью
            return article;
        }

        public async Task IncrementArticleReadsAsync(int id)
        {
            var article = await _articleRepository.GetById(id); // Получаем статью из репозитория

            if (article == null)
            {
                throw new Exception("Статья не найдена."); // Или другое подходящее исключение
            }


            article.ReadsNumber = (article.ReadsNumber ?? 0) + 1; // Увеличиваем счетчик (если null, начинаем с 0)

            // Сохраняем изменения в репозитории (предполагается, что такой метод существует)
            await _articleRepository.UpdateReadsNumber(article.Id, article.ReadsNumber.Value); // Нужно реализовать в репозитории
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
                Annotation = article.Annotation,
                Created = article.Created,
                ReadsNumber = article.ReadsNumber,
                ArticlePreview = article.ArticlePreview,
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