using System;
using System.Collections.Generic;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class FolderStructure
    {
        public long ID { get; set; }
        public string FolderName { get; set; }
        public List<FolderStructure> Children { get; set; }
        public int Count { get; set; }
        public long ParentID { get; set; }
        public bool IsVerified { get; set; }

        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
