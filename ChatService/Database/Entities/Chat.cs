using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Entities;
public class Chat
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid OwnerId { get; set; }
    public bool isDm { get; set; }
    public DateTime CreatedAt { get; set; }
    public required ICollection<ChatMember> Members { get; set; }
}

public class ChatMember
{
    public Guid ChatId { get; set; }
    public required Chat Chat { get; set; }
    public Guid UserId { get; set; }
}