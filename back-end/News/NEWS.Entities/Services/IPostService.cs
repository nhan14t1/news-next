using NEWS.Entities.Models.Dto;
using NEWS.Entities.Models.Responses;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Services
{
    public interface IPostService : IBaseService<Post>
    {
        Task<PostDto> AddAsync(PostVM request, string email, FileManagement thumbnail);

        Task<List<PostDto>> GetAllAsync();

        Task<HomePageResult> GetHomePageData();

        Task<PostDto> GetBySlugAsync(string slug, bool isPreview = false);

        Task<PostDto> GetByIdAsync(int id);

        Task<PostDto> UpdateAsync(PostVM request, FileManagement thumbnail);

        Task DeleteAsync(int id);
        
        Task<List<PostDto>> GetPostMap();

        Task UpdateViews(int postId);
        Task<List<PostDto>> GetRelatedByTagIdsAsync(List<int> tagIds, int excludePostId = 0);
    }
}
