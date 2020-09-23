using System;
using System.Collections.Generic;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class FolderDataInfo
    {
        public long ID { get; set; }
        public long ParentFolderID { get; set; }
        public string Name { get; set; }
        public int CreationUserID { get; set; }
        public DateTime CreationDate { get; set; }

        public string Status { get; set; }

        public List<FolderNamedItem> Directory { get; set; }  //文件或文件夹的目录路径

        public FolderDataInfo()
        {
            Directory = new List<FolderNamedItem>();
        }
    }

    public class FolderNamedItem
    {
        public long ID { get; set; }
        public long? ParentFolderID { get; set; }
        public string Name { get; set; }
    }
}
