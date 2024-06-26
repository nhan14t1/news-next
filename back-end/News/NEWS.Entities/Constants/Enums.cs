﻿namespace NEWS.Entities.Constants
{
    public enum AppRoles
    {
        Admin = 1,
        User = 2,
        Editor = 3
    }

    public enum PostStatus
    {
        Active = 1,
        Schedule = 2,
        Draft = 3,
        Deleted = 4,
    }

    public enum AppCategory
    {
        VietNam = 1,
        Global = 2,
    }
    
    public enum FileType
    {
        PostImage = 1,
        PostThumbnail,
    }
}
