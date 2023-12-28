using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Models.Others
{
    public class BlockedToken
    {
        public int UserId { get; set; }
        public List<string> Tokens { get; set; }
    }
}
