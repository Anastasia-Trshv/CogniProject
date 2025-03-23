using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class Article
{
    public int Id { get; set; }

    public string? ArticleName { get; set; }

    public string? ArticleBody { get; set; }

    public string? Annotation { get; set; }

    public DateTime? Created { get; set; }

    public int? ReadsNumber { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    public virtual User IdUserNavigation { get; set; } = null!;
}
