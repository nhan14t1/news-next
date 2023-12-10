using NEWS.Entities.Models.Others;
using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Models.ViewModels
{
    public class PostVM : Post
    {
        public List<int> CategoryIds { get; set; }
        public List<string> ImageUrls { get; set; }
        public ImageInfo Thumbnail { get; set; }
    }
}
