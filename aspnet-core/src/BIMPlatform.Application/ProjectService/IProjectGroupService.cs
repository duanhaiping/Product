using BIMPlatform.Application.Contracts.Group;
using BIMPlatform.Application.Contracts.ProjectDto;
using System.Collections.Generic;

namespace BIMPlatform.Infrastructure.Project.Services.Interfaces
{
    public interface IProjectGroupService
    {
        void AssignGroupToProject(GroupProjectInfo groupProject);

        void RemoveGroupFromProject(GroupProjectInfo groupProject);


        /// <summary>
        /// Get all groups of the project. Groups are assign to the project
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        IList<GroupDataInfo> GetProjectGroups(int projectID);

        IList<ProjectDto> GetAllProjectsInGroup(int groupID);
    }
}
