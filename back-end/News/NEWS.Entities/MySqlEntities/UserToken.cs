using System;
using System.Collections.Generic;

namespace NEWS.Entities.MySqlEntities;

public partial class UserToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public long ExpirationDate { get; set; }

    public bool IsBlocked { get; set; }

    public virtual User User { get; set; } = null!;
}
