﻿using System;
using System.Collections.Generic;
using Cogni.Database.Entities;

namespace Cogni;

public partial class Customuser
{
    public int IdUser { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Image { get; set; }

    public string? TypeMbti { get; set; }

    public int IdRole { get; set; }

    public int IdMbtiType { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Avatar> Avatars { get; set; } = new List<Avatar>();

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual ICollection<Friend> FriendFriendNavigations { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendUsers { get; set; } = new List<Friend>();

    public virtual MbtiType IdMbtiTypeNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<UserTag> UserTags { get; set; } = new List<UserTag>();

    public Customuser(string? name, string? email, string? password, int idRole, int idMbtiType)
    {
        Name = name;
        Email = email;
        Password = password;
        IdRole = idRole;
        IdMbtiType = idMbtiType;
    }
    public Customuser() { }
}
