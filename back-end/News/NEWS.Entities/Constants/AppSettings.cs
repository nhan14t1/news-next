namespace NEWS.Entities.Constants
{
    public class AppSettings
    {
        public static ConnectionStrings? ConnectionString { get; private set; }
    }

    public class ConnectionStrings
    {
        public static string Default { get; set; }
        public static string Mysql { get; set; }
    }
}
