namespace NEWS.Entities.Constants
{
    public class AppConst
    {
        public static string[] APP_ROLE_NAMES = { "Admin", "User", "Editor" };

        public static class AppRoleNames
        {
            public static string Admin = APP_ROLE_NAMES[0];
            public static string User = APP_ROLE_NAMES[1];
            public static string Editor = APP_ROLE_NAMES[2];
        }

        public static readonly DateTime BASE_DATE = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        public static readonly DateTime BASE_DATE_UTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly long MILISECOND_OF_DATE = 86400000;
        public static readonly string IMAGE_POST_PREFIX = "nqim-";
    }
}
