using System;
using System.Collections.Generic;

namespace NEWS.Entities.MySqlEntities;

public partial class Tag
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public string LowerText { get; set; } = null!;

    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
}
