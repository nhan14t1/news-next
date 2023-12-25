using System;
using System.Collections.Generic;

namespace NEWS.Entities.MySqlEntities;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string Salt { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int Age { get; set; }

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
