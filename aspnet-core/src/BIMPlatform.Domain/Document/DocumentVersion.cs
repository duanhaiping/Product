using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BIMPlatform.Document
{
    public class DocumentVersion : Entity<long>, IAuditedObject, ISoftDelete, IDeletionAuditedObject, IMultiTenant
    {
        public long FolderID { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        [MaxLength(10)]
        public string Suffix { get; set; }
        public string MD5 { get; set; }
        [MaxLength(50)]
        public string Size { get; set; }
        public string RemotePath { get; set; }
        [MaxLength(10)]
        public string Status { get; set; }
        public int CreationUserID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Properties { get; set; }
        public string Tags { get; set; }
        public DateTime CreationTime { get ; set ; }
        public Guid? CreatorId { get ; set ; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get ; set ; }
        public Guid? DeleterId { get ; set ; }
        public DateTime? DeletionTime { get; set; }
        public Guid? TenantId { get; set; }


        public DocumentFolder DocFolder { get; set; }
        public  Document Document { get; set; }
    }
}
