namespace Cogni.Database.Entities;

public partial class Hobby
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TagCategory> Categories { get; set; } = new List<TagCategory>();
}
