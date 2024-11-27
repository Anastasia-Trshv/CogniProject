using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class UserTag
{
    public int Id { get; set; }

    public int IdTag { get; set; }

    public int IdUser { get; set; }

    public virtual Tag IdTagNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
