﻿using NEWS.Entities.Models.Others;
using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Models.Dto
{
    public class PostDto
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

        public ImageInfo Thumbnail { get; set; }

        public long CreatedDate { get; set; }

        public long UpdatedDate { get; set; }

        public long ScheduleDate { get; set; }

        public string UserEmail { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string ThumbnailFileName { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
