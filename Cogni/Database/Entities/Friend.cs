using System;
using System.Collections.Generic;

namespace Cogni;

public partial class Friend
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FriendId { get; set; }

    public DateTime? DateAdded { get; set; }

    public virtual User FriendNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
