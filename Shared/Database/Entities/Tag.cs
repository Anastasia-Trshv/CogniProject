using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class Tag
{
    public int Id { get; set; }

    public string? NameTag { get; set; }

    public int? IdCategory { get; set; }

    public virtual TagCategory? IdCategoryNavigation { get; set; }

    public virtual ICollection<UserTag> UserTags { get; set; } = new List<UserTag>();
}
