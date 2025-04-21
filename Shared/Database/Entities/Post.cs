using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class Post
{
    public int Id { get; set; }

    public string? PostBody { get; set; }

    public int IdUser { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();
}
