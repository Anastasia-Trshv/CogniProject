using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class Article
{
    public int IdArticle { get; set; }

    public string? ArticleName { get; set; }

    public string? ArticleBody { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    public virtual Customuser IdUserNavigation { get; set; } = null!;
}
