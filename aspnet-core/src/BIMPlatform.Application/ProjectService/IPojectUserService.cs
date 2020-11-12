using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Application.Contracts.UserDataInfo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BIMPlatform.Infrastructure.Project.Services.Interfaces
{
    public interface IProjectUserService
    {
        Task<PagedResultDto<UserDto>> GetProjectUserList(BasePagedAndSortedResultRequestDto filter);

        Task<PagedResultDto<ProjectDto>> GetUserProjectList(BasePagedAndSortedResultRequestDto filter);
        Task AddUserToProject(List<Guid> userIdList);
        /// <summary>
        /// 获取可添加成员列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        Task<PagedResultDto<UserDto>> GetTenantUserForAddToProject(BasePagedAndSortedResultRequestDto filter);
        Task DeleteProjectUser( Guid userId);
        
    } 
}
