using BIMPlatform.Application.Contracts.Group;
using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Application.Contracts.UserDataInfo;
using BIMPlatform.Group;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using BIMPlatform.Projects;
using BIMPlatform.Projects.Repositories;
using BIMPlatform.Users;
using BIMPlatform.Users.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using BIMPlatform.ToolKits.Helper;

namespace BIMPlatform.Module.Project.Services.Default
{
    public class ProjectUserRoleService : BaseService, IProjectUserRoleService
    {
        [Dependency]
        public IProjectUserRoleRepository ProjectUserRoleRepository { get; set; }
        [Dependency]
        public IUserInGroupRepository UserInGroupRepository { get; set; }
        public ProjectUserRoleService(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        public virtual void AssignUserProjectRole(ProjectUserRoleDataInfo projectUserRole)
        {
           // ServerLogger.Info(string.Format("Start to assign role {0} to user {1}", projectUserRole.RoleId, projectUserRole.UserId));

            projectUserRole.CreationDate = DateTime.Now;
            ProjectUserRole rel = GetExistingUserRole(projectUserRole);
            if (rel == null)
            {
                rel =ObjectMapper.Map< ProjectUserRoleDataInfo,ProjectUserRole >(projectUserRole);
                ProjectUserRoleRepository.Add(rel);
                this.UnitOfWorkManager.Current.SaveChangesAsync();
            }

           // // ServerLogger.Info("End to assign role to user");
        }

        public virtual void CancelUserProjectRole(ProjectUserRoleDataInfo projectUserRole)
        {
           // // ServerLogger.Info(string.Format("Start to cancel role {0} from user {1}", projectUserRole.RoleId, projectUserRole.UserId));

            ProjectUserRole rel = GetExistingUserRole(projectUserRole);
            if (rel != null)
            {
                ProjectUserRoleRepository.Delete(rel);
                this.UnitOfWorkManager.Current.SaveChangesAsync();
            }

           // // ServerLogger.Info("End to cancel role from user");
        }

        public virtual void AssignUserProjectRole(int projectId, Guid userId, IEnumerable<int> roleIds)
        {
           // // ServerLogger.Info(string.Format("Start to assign roles to user {0}", userId));
            Stopwatch watch = new Stopwatch();
            watch.Start();

            bool addNewRel = false;
            foreach (int roleId in roleIds)
            {
                ProjectUserRole rel = GetExistingUserRole(projectId, userId, roleId);
                if (rel == null)
                {
                    addNewRel = true;
                    ProjectUserRoleRepository.Add(new ProjectUserRole() { ProjectId = projectId, UserId = userId, RoleId = roleId, CreationDate = DateTime.Now });
                }
                else
                {
                    // Already exist, ignore
                }
            }

            if (addNewRel)
            {
                this.UnitOfWorkManager.Current.SaveChangesAsync();
            }

           // // ServerLogger.Info("End to assign roles");
            watch.Stop();
            // ServerLogger.Perfomance(watch, "Assign user roles");
        }

        public virtual void CancelUserProjectRole(int projectId, Guid userId, IEnumerable<int> roleIds)
        {
           // // ServerLogger.Info(string.Format("Start to cancel roles of user {0}", userId));
            Stopwatch watch = new Stopwatch();
            watch.Start();

            bool removeRel = false;
            foreach (int roleId in roleIds)
            {
                ProjectUserRole rel = GetExistingUserRole(projectId, userId, roleId);
                if (rel != null)
                {
                    removeRel = true;
                    ProjectUserRoleRepository.Delete(rel);
                }
                else
                {
                    // no exist
                }
            }

            if (removeRel)
            {
                this.UnitOfWorkManager.Current.SaveChangesAsync();
            }

           //ServerLogger.Info("End to cancel roles");
            watch.Stop();
            // ServerLogger.Perfomance(watch, "Cancel user roles");
        }

        private ProjectUserRole GetExistingUserRole(ProjectUserRoleDataInfo projectUserRole)
        {
            return GetExistingUserRole(projectUserRole.ProjectId, projectUserRole.UserId, projectUserRole.RoleId);
        }

        private ProjectUserRole GetExistingUserRole(int projectId, Guid userId, int roleId)
        {
            return ProjectUserRoleRepository.FindByKeyValues(projectId, userId, roleId);
        }

        /// <summary>
        /// Get users which are assigned to the project
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public IList<UserDto> GetProjectUsers(int projectID)
        {
           // // ServerLogger.Info(string.Format("Start to get assigned users of project {0}", projectID));
            try
            {
                //IList<User> users = ProjectUserRoleRepository.Query(pur => pur.ProjectId == projectID).Select(pur => pur.User).GroupBy(g => g.ID).Select(g => g.FirstOrDefault()).ToList();
                IList<AppUser> users = ProjectUserRoleRepository.Query(pur => pur.ProjectId == projectID).Select(pur => pur.User).Distinct().ToList();
                return ObjectMapper.Map<IList<AppUser>, IList<UserDto>>(users);
            }//
            finally
            {
               // // ServerLogger.Info("End to get assigned user of project");
            }
        }

        public IList<Dictionary<string, object>> GetProjectUnitAndUser(int projectID, string permissionName)
        {
            string[] includedPath = new string[] { "User" };
            IList<Dictionary<string, object>> dics = new List<Dictionary<string, object>>();
            IList<UserInGroup> list = null;
            if (string.IsNullOrEmpty(permissionName))
            {
                list = UserInGroupRepository.FindList();
            }
            else
            {
                list = UserInGroupRepository.FindList();
                //list = UserInGroupRepository.FindList(n => n.User.IsActivated && n.User.ProjectUserRoles1.Any(p => p.ProjectId == projectID && p.Role.RolePermissions.Any(rp => rp.Permission.SystemName == permissionName)));

            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("UserID", item.UserID.ToString());
                    dic.Add("DisplayName", item.User.Name.ToString());
                    dic.Add("MobilePhone", item.User.PhoneNumber.ToString());
                    //dic.Add("Group", ObjectMapper.Map<OrganizationUnit, GroupDataInfo>(item.Group));
                    dics.Add(dic);
                }
            }
            return dics;
        }
    }
}
