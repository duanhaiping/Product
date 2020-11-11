using BIMPlatform.Application.Contracts.UserDataInfo;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BIMPlatform.Application.Contracts.ProjectDto
{
    public class ProjectDto: AuditedEntityDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string Address { get; set; }
        public decimal ProjectEstimate { get; set; }
        public string ConstructionUnit { get; set; }
        public string MainContractor { get; set; }
        public string DesignOrganization { get; set; }
        public string SupervisingUnit { get; set; }
        public string ConsultingUnit { get; set; }
        public float Area { get; set; }
        public Guid Principal { get; set; }
        public DateTime CompleteDate { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 完工倒计时天数
        /// </summary>
        public int CompleteCountDown { get; set; }
        public UserDto PrincipalInfo { get; set; }
        public List<string> PreviewRelativePaths { get; set; }
        /// <summary>
        /// 是否属于当前项目
        /// </summary>
        public bool IsOwn { get; set; }
        /// <summary>
        /// 项目封面图片信息
        /// </summary>
        public string coverImagePath { get; set; }
    }
}
