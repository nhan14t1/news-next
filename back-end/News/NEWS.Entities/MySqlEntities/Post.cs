using System;
using System.Collections.Generic;

namespace NEWS.Entities.MySqlEntities;

public partial class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string IntroText { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int Views { get; set; }

    public int UserId { get; set; }

    public int Status { get; set; }

    public int? ThumbnailId { get; set; }

    public long CreatedDate { get; set; }

    public long UpdatedDate { get; set; }

    public long ScheduleDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();

    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

    public virtual FileManagement? Thumbnail { get; set; }

    public virtual User User { get; set; } = null!;
}
