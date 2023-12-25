using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Models.ViewModels
{
    public class UserVM : User
    {
        public int RoleId { get; set; }
    }
}
