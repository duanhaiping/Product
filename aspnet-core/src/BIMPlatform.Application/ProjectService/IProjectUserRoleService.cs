using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Application.Contracts.UserDataInfo;
using System;
using System.Collections.Generic;

namespace BIMPlatform.Infrastructure.Project.Services.Interfaces
{
    public interface IProjectUserRoleService
    {
        void AssignUserProjectRole(int projectId, Guid userId, IEnumerable<int> roleIds);
        void AssignUserProjectRole(ProjectUserRoleDataInfo ProjectUserRole);

        void CancelUserProjectRole(int projectId, Guid userId, IEnumerable<int> roleIds);

        void CancelUserProjectRole(ProjectUserRoleDataInfo ProjectUserRole);

        /// <summary>
        /// Get users which are assigned to the project
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        IList<UserDto> GetProjectUsers(int projectID);

        IList<Dictionary<string, object>> GetProjectUnitAndUser(int projectID, string permissionName);
    }
}
