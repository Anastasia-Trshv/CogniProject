namespace Cogni.Database.Entities;

public partial class UserTagCategory
{
    public int Id { get; set; }

    public int IdCategory { get; set; }

    public int IdUser { get; set; }

    public virtual TagCategory IdCategoryNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
