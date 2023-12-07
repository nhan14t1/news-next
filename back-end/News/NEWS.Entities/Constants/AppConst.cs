namespace NEWS.Entities.Constants
{
    public class AppConst
    {
        public static string[] APP_ROLES = { "Admin", "User" };

        public static class AppRoles
        {
            public static string Admin = APP_ROLES[0];
            public static string User = APP_ROLES[1];
        }

        public static readonly DateTime BASE_DATE = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        public static readonly DateTime BASE_DATE_UTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly long MILISECOND_OF_DATE = 86400000;
    }

    public enum AppRoles
    {
        Admin = 1,
        User = 2,
    }

    public enum PostStatus
    {
        Active = 1,
        Schedule = 2,
        Draft = 3,
    }

    public enum AppCategory
    {
        VietNam = 1,
        Global = 2,
    }
}
