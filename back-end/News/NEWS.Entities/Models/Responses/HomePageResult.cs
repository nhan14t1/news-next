using NEWS.Entities.Models.Dto;

namespace NEWS.Entities.Models.Responses
{
    public class HomePageResult
    {
        public List<PostDto> VietNamPosts { get; set; }
        public List<PostDto> GlobalPosts { get; set; }
        public List<PostDto> VideoPosts { get; set; }
        public List<PostDto> TopPosts { get; set; }
    }
}
