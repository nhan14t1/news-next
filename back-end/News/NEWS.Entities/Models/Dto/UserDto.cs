namespace NEWS.Entities.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int Age { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
