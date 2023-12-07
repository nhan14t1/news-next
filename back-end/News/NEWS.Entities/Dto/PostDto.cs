using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Dto
{
    public class PostDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string IntroText { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Content { get; set; } = null!;

        public int UserId { get; set; }

        public int Status { get; set; }

        public long CreatedDate { get; set; }

        public long UpdatedDate { get; set; }

        public long ScheduleDate { get; set; }

        public string UserEmail { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
