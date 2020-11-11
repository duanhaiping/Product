using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.UserDataInfo;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using BIMPlatform.ProjectService;
using Microsoft.AspNetCore.Mvc;
using Platform.ToolKits.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIMPlatform.Controllers.Project
{
    [ApiExplorerSettings(GroupName = ApiGrouping.GroupName_v4)]
    public class ProjectUserController : BaseController
    {
        protected IProjectUserService ProjectUserService { get; set; }
        protected IProjectService ProjectService { get; set; }

        public ProjectUserController(
            IProjectUserService projectUserService,
            IProjectService projectService)
        {
            ProjectUserService = projectUserService;
            ProjectService = projectService;
        }
        /// <summary>
        /// 获取当前用户的项目信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResult> GetUserProjectInfo()
        {

            return await ServiceResult.IsSuccess();
        }

        [HttpGet]
        public async Task<ServiceResult> GetAddToProjectUserList([FromQuery]BasePagedAndSortedResultRequestDto filter)
        {
            var users =await ProjectUserService.GetTenantUserForAddToProject(filter);
            // return await ServiceResult<IList<ProjectDto>>.PageList(project.Items.ToList(), project.TotalCount, string.Empty);
            return await ServiceResult<IList<UserDto>>.PageList(users.Items.ToList(), users.TotalCount, string.Empty);
        }
        [HttpGet]
        public async Task<ServiceResult> GetProjectUserList([FromQuery]BasePagedAndSortedResultRequestDto filter)
        {
            var users = await ProjectUserService.GetProjectUserList(filter);
            // return await ServiceResult<IList<ProjectDto>>.PageList(project.Items.ToList(), project.TotalCount, string.Empty);
            return await ServiceResult<IList<UserDto>>.PageList(users.Items.ToList(), users.TotalCount, string.Empty);
        }
        [HttpPost]
        public async Task<ServiceResult> AddUserToProject([FromBody] List<Guid> userIdList)
        {
            await ProjectUserService.AddUserToProject(userIdList);
            return await ServiceResult.IsSuccess();
        }
        [HttpDelete]
        public async Task<ServiceResult> DeleteProjectUser([FromQuery]Guid userId)
        {
            await ProjectUserService.DeleteProjectUser(  userId);
            return await ServiceResult.IsSuccess();
        }
        
    }
}
