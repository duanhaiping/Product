using BIMPlatform.Application.Contracts.Group;
using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Application.Contracts.UserDataInfo;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using BIMPlatform.Projects.Repositories;
using BIMPlatform.Users;
using BIMPlatform.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;

namespace BIMPlatform.Module.Project.Services.Default
{

    public class ProjectUserRolePermissionService :BaseService, IProjectUserRolePermissionService
    {
       

        [Dependency]
        public IUserInGroupRepository UserInGroupRepository { get; set; }

        [Dependency]
        public IProjectGroupRepository ProjectGroupRepository { get; set; }

        [Dependency]
        public IProjectUserRoleRepository ProjectUserRoleRepository { get; set; }

        public ProjectUserRolePermissionService(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        public virtual IList<PermissionGrantInfoDto> GetProjectPermissions(int projectID, int userID)
        {
            //ServerLogger.Info(string.Format("Get user's project permissions by project id {0} and user id {1}", projectID, userID));
            IList<PermissionGrant> projectPermissions =
                PermissionRepository.FindList(p => p.RolePermissions.Any(rp => rp.Role.ProjectUserRoles.Any(pur => pur.UserId == userID && pur.ProjectId == projectID)));
            return ObjectMapper.Map<PermissionGrant, PermissionGrantInfoDto>(projectPermissions);
        }

        public virtual IList<PermissionGrantInfoDto> GetAllPermissions(int projectID, int userID)
        {
            //ServerLogger.Info(string.Format("Get user's all permissions, including system permissions, by project id {0} and user id {1}", projectID, userID));
            List<PermissionGrantInfoDto> allPermissions = new List<PermissionGrantInfoDto>();
            
            IList<PermissionGrantInfoDto> systemPermissions = base.GetSystemPermissionsByUserID(userID);
            allPermissions.AddRange(systemPermissions);

            IList<PermissionGrantInfoDto> projectPermissions = GetProjectPermissions(projectID, userID);
            allPermissions.AddRange(projectPermissions);
            return allPermissions;
        }

        public virtual IList<RoleDataInfo> GetProjectRolesByUserID(int projectID, int userID)
        {
            //ServerLogger.Info(string.Format("Get user's project roles by project id {0} and user id {1}", projectID, userID));
            IList<Role> roles = RoleRepository.FindList(r => r.ProjectUserRoles.Any(u => u.ProjectId == projectID && u.UserId == userID));
            return ObjectMapper.Map<Role, RoleDataInfo>(roles);
        }

        public virtual IList<RoleDataInfo> GetAllRoles(long tenant, int projectID, int userID)
        {
            //ServerLogger.Info(string.Format("Get user's all roles, including system roles, by project id {0} and user id {1}", projectID, userID));

            List<RoleDataInfo> allRoles = new List<RoleDataInfo>();

            IList<RoleDataInfo> systemRoles = base.GetSystemRolesByUserID(tenant, userID);
            allRoles.AddRange(systemRoles);

            IList<RoleDataInfo> projectRoles = GetProjectRolesByUserID(projectID, userID);
            allRoles.AddRange(projectRoles);
            return allRoles;
        }

        public virtual IList<PermissionGrantInfoDto> GetPermissionByType(string type)
        {
            //ServerLogger.Info(string.Format("Get all {0} permissions", type));
            return base.GetPermissionsByType(type);
        }

        public virtual ViewProjectDataInfo CheckViewProjectPermissions(int projectID, int userID)
        {
            ViewProjectDataInfo viewProInfo = new ViewProjectDataInfo();
            IList<PermissionGrantInfoDto> allPermissions = this.GetAllPermissions(projectID, userID);
            viewProInfo.ViewAllProjects = allPermissions.FirstOrDefault(p => p.SystemName == "ViewAllProjects") != null;
            viewProInfo.ViewOwnedProjects = allPermissions.FirstOrDefault(p => p.SystemName == "ViewOwnedProject") != null;
            return viewProInfo;
        }

        public virtual IList<UserDto> GetUsersByProjectPermission(int projectID, IList<string> permissions)
        {
            IEnumerable<int> permissionIDs = PermissionRepository.Query(p => p.Type == "Project" && permissions.Any(n => n.Contains(p.SystemName) || p.SystemName.Contains(n))).Select(p => p.ID);
            IList<AppUser> users = ProjectUserRoleRepository.Query(pur => pur.ProjectId == projectID &&
                                pur.Role.RolePermissions.Any(rp => permissionIDs.Contains(rp.PermissionId))).Select(pur => pur.User).ToList();
            return ObjectMapper.Map<AppUser, UserDto>(users);
        }

        public bool HasProjectPermission(int projectID, int userID, string pemissionSystemName)
        {
            IEnumerable<int> permissionIDs = PermissionRepository.Query(p => p.Type == "Project" && p.SystemName == pemissionSystemName).Select(p => p.ID);
            int userRoleCount = ProjectUserRoleRepository.Query(pur => pur.ProjectId == projectID && pur.UserId == userID &&
                                pur.Role.RolePermissions.Any(rp => permissionIDs.Contains(rp.PermissionId))).Count();
            return userRoleCount > 0;
        }
        public bool HasProjectPermission(int projectID, int userID, IList<string> pemissionSystemNames)
        {
            IEnumerable<int> permissionIDs = PermissionRepository.Query(p => p.Type == "Project" && pemissionSystemNames.Contains(p.SystemName)).Select(p => p.ID);
            int userRoleCount = ProjectUserRoleRepository.Query(pur => pur.ProjectId == projectID && pur.UserId == userID &&
                                pur.Role.RolePermissions.Any(rp => permissionIDs.Contains(rp.PermissionId))).Count();
            return userRoleCount > 0;
        }

        public IList<GroupDataInfo> GetGroupsByPermission(int projectID, IEnumerable<string> permissionNames)
        {
            IList<int> permissionIDs =
                PermissionRepository.Query(p => p.Type == "Project" && permissionNames.Contains(p.SystemName)).Select(p => p.ID).ToList();
            IList<int> userIDs = ProjectUserRoleRepository.QuerUsery(pur => pur.ProjectId == projectID && pur.User.IsActivated &&
                                pur.Role.RolePermissions.Any(rp => permissionIDs.Contains(rp.PermissionId))).Select(pur => pur.UserId).ToList();
            IList<int> groupIDs = ProjectGroupRepository.Query(pg => pg.ProjectID == projectID).Select(pg => pg.GroupID).ToList();
            IList<Group.Group> groupsResult = UserInGroupRepository.Query(ug => userIDs.Contains(ug.UserId) && groupIDs.Contains(ug.GroupID)).Select(ug => ug.Group).Distinct().ToList();
            return ObjectMapper.Map<Group.Group, GroupDataInfo>(groupsResult);
        }

        public IList<UserDto> GetUsersByGroupAndPermission(int projectID, int groupID, IEnumerable<string> permissionNames)
        {
            bool groupOnProject = ProjectGroupRepository.FirstOrDefault(pg => pg.ProjectID == projectID && pg.GroupID == groupID) != null;
            if (groupOnProject)
            {
                IList<int> permissionIDs = PermissionRepository.Query(p => p.Type == "Project" && permissionNames.Contains(p.SystemName)).Select(p => p.ID).ToList();
                IList<int> groupUserIDs = UserInGroupRepository.Query(ug => ug.GroupID == groupID && ug.User.IsActivated).Select(ug => ug.UserID).ToList();
                IList<AppUser> usersResult = ProjectUserRoleRepository.Query(pur => pur.ProjectId == projectID && groupUserIDs.Contains(pur.UserId) &&
                                        pur.Role.RolePermissions.Any(rp => permissionIDs.Contains(rp.PermissionId))).Select(pur => pur.User).Distinct().ToList();
                return ObjectMapper.Map<AppUser, UserDto>(usersResult);
            }
            else
            {
                return new List<UserDto>();
            }
        }
    }
}
