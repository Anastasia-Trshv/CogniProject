using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string? NameRole { get; set; }

    public virtual ICollection<User> Customusers { get; set; } = new List<User>();

    public Role(int idRole, string? nameRole)
    {
        Id = idRole;
        NameRole = nameRole;
    }
    public Role() { }
}
