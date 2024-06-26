﻿using NEWS.Entities.Models.Dto;

namespace NEWS.Entities.Models.Responses
{
    public class PostResult
    {
        public PostDto Post { get; set; }
        public List<PostDto> RelatedPosts { get; set; }
        public HomePageResult RelatedData { get; set; }
    }
}
