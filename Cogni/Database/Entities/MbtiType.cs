using System;
using System.Collections.Generic;

namespace Cogni;

public partial class MbtiType
{
    public int IdMbtiType { get; set; }

    public string? NameOfType { get; set; }

    public virtual ICollection<Customuser> Customusers { get; set; } = new List<Customuser>();
}
