﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BIMPlatform.Document
{
    public class DocumentFolder : Entity<long>, IAuditedObject, ISoftDelete, IDeletionAuditedObject, IMultiTenant
    {
        public long? ParentFolderID { get; set; }
        public string Name { get; set; }
        public int CreationUserID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public Guid? RecycleIdentity { get; set; }
        public DateTime CreationTime { get ; set ; }
        public Guid? CreatorId { get ; set ; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get ; set ; }
        public Guid? DeleterId { get ; set ; }
        public DateTime? DeletionTime { get; set; }
        public Guid? TenantId { get; set; }

        public  ICollection<Document> Documents { get; set; }
        public  ICollection<DocumentVersion> DocumentVersions { get; set; }
        public virtual ICollection<DocumentFolder> DocFolder1 { get; set; }
        public virtual DocumentFolder DocFolder2 { get; set; }
    }
}