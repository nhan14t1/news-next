using NEWS.Entities.Models.Dto;
using NEWS.Entities.Models.Responses;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Services
{
    public interface IPostService : IBaseService<Post>
    {
        Task<Post> AddAsync(PostVM request, string email, FileManagement thumbnail);

        Task<List<PostDto>> GetAllAsync();

        Task<HomePageResult> GetHomePageData();

        Task<PostDto> GetBySlugAsync(string slug);
     
        Task<PostDto> GetByIdAsync(int id);

        Task<PostDto> UpdateAsync(PostVM request);
    }
}
