using System;
using System.Collections.Generic;

namespace Cogni;

public partial class Post
{
    public int IdPost { get; set; }

    public string? PostBody { get; set; }

    public int IdUser { get; set; }

    public virtual Customuser IdUserNavigation { get; set; } = null!;

    public virtual ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();
}
