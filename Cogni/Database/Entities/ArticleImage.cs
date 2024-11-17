using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class ArticleImage
{
    public int Id { get; set; }

    public int ArticleId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual Article Article { get; set; } = null!;
}
