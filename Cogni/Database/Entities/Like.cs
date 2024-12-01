using System;
using System.Collections.Generic;

namespace Cogni;

public partial class Like
{
    public int? UserId { get; set; }

    public int? PostId { get; set; }

    public DateTime? LikedAt { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
