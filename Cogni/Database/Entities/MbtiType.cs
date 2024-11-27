using System;
using System.Collections.Generic;

namespace Cogni;

public partial class MbtiType
{
    public int IdMbtiType { get; set; }

    public string? NameOfType { get; set; }

    public virtual ICollection<User> Customusers { get; set; } = new List<User>();
}
