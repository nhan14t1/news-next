using NEWS.Entities.Dto;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Responses;
using NEWS.Entities.ViewModels;

namespace NEWS.Entities.Services
{
    public interface IPostService : IBaseService<Post>
    {
        Task<Post> AddAsync(PostVM request, string email);

        Task<List<PostDto>> GetAllAsync();

        Task<HomePageResult> GetHomePageData();
    }
}
