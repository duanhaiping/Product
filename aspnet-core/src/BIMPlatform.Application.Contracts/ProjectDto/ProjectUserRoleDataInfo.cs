using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.ProjectDto
{
    public class ProjectUserRoleDataInfo
    {
        public Guid UserId { get; set; }
        public int ProjectId { get; set; }
        public int RoleId { get; set; }
        public System.DateTime CreationDate { get; set; }

        public int CreationUserID { get; set; }

        public string ProjectName { get; set; }
        public string RoleName { get; set; }
    }
}
