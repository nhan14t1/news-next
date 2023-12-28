namespace NEWS.Entities.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Phiên đăng nhập đã hết hạn") { }
        public UnauthorizedException(string message) : base(message) { }
    }
}
