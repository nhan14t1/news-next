using System;
using System.Collections.Generic;

namespace NEWS.Entities.MySqlEntities;

public partial class FileManagement
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Extension { get; set; } = null!;

    public int Type { get; set; }

    public long CreatedDate { get; set; }

    public bool IsUsed { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
