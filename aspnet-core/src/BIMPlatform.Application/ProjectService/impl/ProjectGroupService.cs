using BIMPlatform.Application.Contracts.Group;
using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using BIMPlatform.Projects;
using BIMPlatform.Projects.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace BIMPlatform.Module.Project.Services.Default
{
    public class ProjectGroupService : BaseService, IProjectGroupService
    {
        [Dependency]
        public IProjectGroupRepository ProjectGroupRepository { get; set; }

        public ProjectGroupService(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

    public virtual void AssignGroupToProject(GroupProjectInfo groupProject)
        {
           // // ServerLogger.Info(string.Format("Start to assign group {0} to project {1}", groupProject.GroupID, groupProject.ProjectID));

            ProjectGroup rel = GetExistingProjectGroup(groupProject);
            if (rel == null)
            {
                rel = new ProjectGroup() { ProjectID = groupProject.ProjectID, GroupID = groupProject.GroupID, CreationDate = DateTime.Now };
                ProjectGroupRepository.Add(rel);
               
            }

           // // ServerLogger.Info("End to assign assign group to project");
        }

        public virtual void RemoveGroupFromProject(GroupProjectInfo groupProject)
        {
           // // ServerLogger.Info(string.Format("Start to remove group {0} from project {1}", groupProject.GroupID, groupProject.ProjectID));

            ProjectGroup rel = GetExistingProjectGroup(groupProject);
            if (rel != null)
            {
                ProjectGroupRepository.Delete(rel);
               
            }

           // // ServerLogger.Info("End to remove group from project");
        }

        private ProjectGroup GetExistingProjectGroup(GroupProjectInfo groupProject)
        {
            return GetExistingProjectGroup(groupProject.ProjectID, groupProject.GroupID);
        }

        private ProjectGroup GetExistingProjectGroup(int projectID, int groupID)
        {
            return ProjectGroupRepository.FindByKeyValues(projectID, groupID);
        }

        /// <summary>
        /// Get assigned groups of the project
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public IList<GroupDataInfo> GetProjectGroups(int projectID)
        {
           // // ServerLogger.Info(string.Format("Start to get assigned groups of project {0}", projectID));
            try
            {
                IList<Group.Group> groups = ProjectGroupRepository.Query(pg => pg.ProjectID == projectID).Select(pg => pg.Group).ToList();
                return ObjectMapper.Map<IList<Group.Group>, IList<GroupDataInfo>>(groups);
            }
            finally
            {
               // // ServerLogger.Info("End to get assigned groups of project");
            }
        }

        public IList<ProjectDto> GetAllProjectsInGroup(int groupID)
        {
            IList<Projects.Project> projects = ProjectGroupRepository.Query(pg => pg.GroupID == groupID).Select(pg => pg.Project).ToList();
            return ObjectMapper.Map<IList<Projects.Project>, IList<ProjectDto>>(projects);
        }
    }
}
