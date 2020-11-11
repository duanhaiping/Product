using BIMPlatform.Application.Contracts.ProjectDto;

namespace BIMPlatform.Infrastructure.Project.Services.Interfaces
{
    public interface IGroupProjectService
    {
        void AddGroupProject(GroupProjectInfo groupProject);

        void DeleteGroupProject(int groupId, int projectId);
    }
}
