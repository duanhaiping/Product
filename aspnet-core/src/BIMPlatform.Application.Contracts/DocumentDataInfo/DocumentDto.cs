using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DocumentDto : AuditedEntityDto<Guid>
    {
        public long FolderID { get; set; }
        public string Name { get; set; }
        public string DocNumber { get; set; }
        public string Suffix { get; set; }
        public string Properties { get; set; }
        public string Status { get; set; }
        public Guid RecycleIdentity { get; set; }
    }
}
