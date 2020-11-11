using BIMPlatform.Application.Contracts.Group;
using BIMPlatform.Application.Contracts.UserDataInfo;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace BIMPlatform.Application.Contracts.ProjectDto
{
    public class UserInProjectDto: AuditedEntityDto<long>
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string MobilePhone { get; set; }
        public string UserHeadImgUrl { get; set; }

        public bool IsSystemAdministrator { get; set; }
        public GroupDataInfo InAndExGroup { get; set; }

        public ICollection<LightProjectDataInfo> AssignedProjects { get; set; }

        /// <summary>
        ///  Current assigned projects
        /// </summary>
        //public IList<ProjectUserRoleDataInfo> ProjectRoles { get; set; }

        /// <summary>
        /// Get Last Selected Project, 
        /// if there is selected project, client could set one from assign projects
        /// </summary>
        public LightProjectDataInfo CurrentProject { get; set; }

        public ICollection<PermissionGrantInfoDto> CurrentProjectPermissions { get; set; }
        public ICollection<PermissionGrantInfoDto> SystemPermissions { get; set; }
    }
    public class LightProjectDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int CreationUserID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Address { get; set; }
        public decimal ProjectEstimate { get; set; }
        public IList<IdentityRoleDto> ProjectRoles { get; set; }
    }
}
