using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Application.Contracts.UserDataInfo;
using System;
using System.Collections.Generic;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace BIMPlatform.Infrastructure.Project.Services.Interfaces
{
    public interface IProjectUserRolePermissionService /*: IUserRolePermissionService*/
    {
        /// <summary>
        /// Get the permissions of assigned of the role for the user and current project
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<PermissionGrantInfoDto> GetProjectPermissions(int projectID, Guid userID);

        /// <summary>
        /// Get all permissions of assigned of the role for the user and current project, including system role
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<PermissionGrantInfoDto> GetAllPermissions(int projectID, int userID);

        /// <summary>
        /// Get the permissions of assigned of the role for the user and current project
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<IdentityRoleDto> GetProjectRolesByUserID(int projectID, int userID);

        /// <summary>
        /// Get all permissions of assigned of the role for the user and current project, including system role
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<IdentityRoleDto> GetAllRoles(long tenant, int projectID, int userID);

        IList<PermissionGrantInfoDto> GetPermissionByType(string type);

        /// <summary>
        /// Check whether user can view project
        /// </summary>
        /// <returns></returns>
        ViewProjectDataInfo CheckViewProjectPermissions(int projectID, int userID);

        IList<UserDto> GetUsersByProjectPermission(int projectID, IList<string> permissions);

        bool HasProjectPermission(int projectID, int userID, string pemissionSystemName);

        bool HasProjectPermission(int projectID, int userID, IList<string> pemissionSystemNames);

        IList<OrganizationUnit> GetGroupsByPermission(int projectID, IEnumerable<string> permissionNames);

        IList<UserDto> GetUsersByGroupAndPermission(int projectID, int groupID, IEnumerable<string> permissionNames);
        ICollection<PermissionGrantInfoDto> GetSystemPermissionsByUserID(Guid userID);
    }
}
