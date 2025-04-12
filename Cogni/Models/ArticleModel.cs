using System;
using System.Collections.Generic;
using Cogni.Database.Entities;

namespace Cogni.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }

        public string? ArticleName { get; set; }

        public string? ArticleBody { get; set; }

        public int IdUser { get; set; }

        public string? Annotation { get; set; }

        public DateTime? Created { get; set; }

        public int? ReadsNumber { get; set; }

        public List<ArticleImageModel> ArticleImages { get; set; } = new List<ArticleImageModel>();

        public User? IdUserNavigation { get; set; }

        public ArticleModel(int id, string? articleName, string? articleBody, int idUser, List<ArticleImageModel> articleImages, User? idUserNavigation, string? annotation, DateTime? created, int? readsNumber)
        {
            Id = id;
            ArticleName = articleName;
            ArticleBody = articleBody;
            IdUser = idUser;
            ArticleImages = articleImages;
            IdUserNavigation = idUserNavigation;
            Annotation = annotation;
            Created = created;
            ReadsNumber = readsNumber;
        }

        public ArticleModel() { }
    }
}