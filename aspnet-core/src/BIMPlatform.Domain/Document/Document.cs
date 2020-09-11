using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BIMPlatform.Document
{
    public class Document : Entity<Guid>, IAuditedObject, ISoftDelete, IDeletionAuditedObject, IMultiTenant
    {
        public long FolderID { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        public string DocNumber { get; set; }
        [MaxLength(10)]
        public string Suffix { get; set; }
        public string Properties { get; set; }
        public string Status { get; set; }
        public Guid RecycleIdentity { get; set; }
        public DateTime CreationTime { get ; set ; }
        public Guid? CreatorId { get ; set ; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get ; set ; }
        public Guid? DeleterId { get ; set ; }
        public DateTime? DeletionTime { get; set; }
        public Guid? TenantId { get; set; }
    }
}
