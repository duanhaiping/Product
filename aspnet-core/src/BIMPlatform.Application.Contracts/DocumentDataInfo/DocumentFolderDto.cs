using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DocumentFolderDto
    {
        public long Id { get; set; }
        public string FolderName { get; set; }
        public List<DocumentFolderDto> Children { get; set; }
        public int Count { get; set; }
        public long ParentID { get; set; }
        public bool IsVerified { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
