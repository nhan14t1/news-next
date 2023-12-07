using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.ViewModels
{
    public class PostVM : Post
    {
        public List<int> CategoryIds {get; set; }
    }
}
