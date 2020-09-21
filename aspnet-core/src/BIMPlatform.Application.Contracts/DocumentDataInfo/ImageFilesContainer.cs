using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class ImageFilesContainer
    {
        public List<ImageFileItem> Items { get; private set; }
        public ImageFilesContainer()
        {
            Items = new List<ImageFileItem>();
        }
    }

    public class ImageFileItem : ImageFile
    {
        public long VersionID { get; set; }
    }

    public class ImageFile
    {
        public string ThumbnailPath { get; set; }
        public string MediumImagePath { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailRelativePath { get; set; }
        public string MediumImageRelativePath { get; set; }
        public string ImageRelativePath { get; set; }
    }
}
