using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.ProjectDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BIMPlatform.ProjectService
{
    public interface IProjectService 
    {
        Task<PagedResultDto<ProjectDto>> GetProjects(BasePagedAndSortedResultRequestDto filter);
        Task<IList<ProjectDto>> GetProjects();
        Task<ProjectDto> GetProjectAsync(int projectID);
        Task CreateAsync(ProjectCreateParams projectDto);
        Task<ProjectDto> UpdateAsync(ProjectUpdateParams projectDto);
        Task DeleteAsync(int projectID);
        Task<ProjectDto> GetCurrentUserProject();
    }
}
