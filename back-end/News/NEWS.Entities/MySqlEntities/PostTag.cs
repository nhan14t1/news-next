using System;
using System.Collections.Generic;

namespace NEWS.Entities.MySqlEntities;

public partial class PostTag
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int TagId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
