using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class Role
{
    public int IdRole { get; set; }

    public string? NameRole { get; set; }

    public virtual ICollection<Customuser> Customusers { get; set; } = new List<Customuser>();

    public Role(int idRole, string? nameRole)
    {
        IdRole = idRole;
        NameRole = nameRole;
    }
    public Role() { }
}
