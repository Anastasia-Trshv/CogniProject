using System;
using System.Collections.Generic;
using Cogni.Database.Entities;

namespace Cogni;

public partial class Message
{
    public int Id { get; set; }

    public int AvatarId { get; set; }

    public string MessageBody { get; set; } = null!;

    public int ChatId { get; set; }

    public string? AttachmentUrl { get; set; }

    public virtual Avatar Avatar { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;
}
