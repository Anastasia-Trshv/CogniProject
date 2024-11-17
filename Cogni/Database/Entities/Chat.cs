using System;
using System.Collections.Generic;

namespace Cogni.Database.Entities;

public partial class Chat
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Customuser User { get; set; } = null!;
}
