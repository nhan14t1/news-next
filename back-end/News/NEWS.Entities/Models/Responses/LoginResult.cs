using NEWS.Entities.MySqlEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Entities.Models.Responses
{
    public class LoginResult
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Role> Roles { get; set; }

        public string AccessToken { get; set; }

        public long TokenExpirationInTimeStamp { get; set; }
    }
}
