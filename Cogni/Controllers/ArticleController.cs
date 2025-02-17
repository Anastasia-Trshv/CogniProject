using Microsoft.AspNetCore.Mvc;
using Cogni.Contracts;
using Cogni.Services;
using Cogni.Database.Entities;
using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _articleService.GetAll();
            var articlesResponse = articles.Select(a => new ArticlesResponse(a.Id, a.ArticleName)).ToList();
            return Ok(articlesResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var imageUrls = article.ArticleImages.Select(i => i.ImageUrl).ToList();

            var articleResponse = new ArticleResponse(article.Id, article.ArticleName, article.ArticleBody, imageUrls, article.IdUser);

            return Ok(articleResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequest request)
        {
            if (string.IsNullOrEmpty(request.ArticleName) || string.IsNullOrEmpty(request.ArticleBody))
            {
                return BadRequest("Название статьи и текст не могут быть пустыми.");
            }

            await _articleService.CreateArticle(request.ArticleName, request.ArticleBody, request.ImageUrls, request.UserId);
            return CreatedAtAction(nameof(GetArticleById), new { id = request.IdArticle }, request);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArticle(int id, ArticleUpdateRequest request)
        {
            if (id != request.IdArticle)
            {
                return BadRequest("Несовпадение Id в пути и в теле запроса.");
            }

            await _articleService.Update(request.IdArticle, request.ArticleName, request.ArticleBody, request.ImageUrls);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            await _articleService.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}

