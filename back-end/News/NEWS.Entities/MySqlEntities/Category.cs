using System;
using System.Collections.Generic;

namespace NEWS.Entities.MySqlEntities;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int IsActive { get; set; }

    public virtual ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}
