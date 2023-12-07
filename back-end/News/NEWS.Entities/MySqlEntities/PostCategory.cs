using System;
using System.Collections.Generic;

namespace NEWS.Entities.MySqlEntities;

public partial class PostCategory
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
