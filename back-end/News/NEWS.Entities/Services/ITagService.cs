using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Services
{
    public interface ITagService : IBaseService<Tag>
    {
        Task<List<Tag>> SearchAsync(string keyword);
    }
}
