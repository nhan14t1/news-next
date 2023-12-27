using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Models.ViewModels
{
    public class UserVM : User
    {
        public List<int> RoleIds { get; set; }
    }
}
