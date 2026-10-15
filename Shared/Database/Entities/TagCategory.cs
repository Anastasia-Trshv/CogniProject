namespace Cogni.Database.Entities;

public partial class TagCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int IdHobby { get; set; }

    public virtual Hobby IdHobbyNavigation { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<UserTagCategory> UserTagCategories { get; set; } = new List<UserTagCategory>();
}
