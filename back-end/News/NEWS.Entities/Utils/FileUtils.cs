using NEWS.Entities.Constants;
using NEWS.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NEWS.Entities.Constants.AppConst;

namespace NEWS.Entities.Utils
{
    public class FileUtils
    {
        public static string GetFolderPath(FileType type)
        {
            switch (type)
            {
                case FileType.PostImage: return FileTypeFolderPath.PostImage;
                case FileType.PostThumbnail: return FileTypeFolderPath.PostThumbnail;
            }

            throw new BusinessException("Không tìm thấy loại file");
        }
        
        public static string GetFolderPath(int typeId)
        {
            try
            {
               var type = (FileType)typeId;
                return GetFolderPath(type);
            }
            catch (Exception e) {
                throw new BusinessException("Không tìm thấy loại file");
            }
        }
    }
}
