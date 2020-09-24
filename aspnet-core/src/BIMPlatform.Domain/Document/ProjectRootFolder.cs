using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace BIMPlatform.Document
{
    public class ProjectRootFolder : Entity<long>, IAuditedObject, ISoftDelete, IDeletionAuditedObject
    {
        public long FolderID { get; set; }
        public string Type { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? TenantId { get; set; }

        public  DocumentFolder DocFolder { get; set; }
        public  Projects.Project Project { get; set; }
    }
}
