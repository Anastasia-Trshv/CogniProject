using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Entities;

public class Chat
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int OwnerId { get; set; }
    public bool isDm { get; set; }
    public DateTime CreatedAt { get; set; }
    public required ICollection<ChatMember> Members { get; set; }
}

public class ChatMember
{
    public Guid ChatId { get; set; }
    public required Chat Chat { get; set; }
    public int UserId { get; set; }
}