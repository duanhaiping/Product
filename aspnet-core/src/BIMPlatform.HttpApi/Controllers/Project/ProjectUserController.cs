using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.UserDataInfo;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using BIMPlatform.ProjectService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.ToolKits.Base;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIMPlatform.Controllers.Project
{
    [Area("project")]
    [Route("api/project/users")]
    [ApiExplorerSettings(GroupName = ApiGrouping.GroupName_v4)]
    [AllowAnonymous]
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
        /// 可增加到项目的用户
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("page/pendding")]
        [SwaggerResponse(200, "", typeof(ServiceResult<IList<UserDto>>))]
        public async Task<ServiceResult> GetAddToProjectUserList([FromQuery]BasePagedAndSortedResultRequestDto filter)
        {
            var users =await ProjectUserService.GetTenantUserForAddToProject(filter);
            // return await ServiceResult<IList<ProjectDto>>.PageList(project.Items.ToList(), project.TotalCount, string.Empty);
            return await ServiceResult<IList<UserDto>>.PageList(users.Items.ToList(), users.TotalCount, string.Empty);
        }
        /// <summary>
        /// 已存在的项目成员
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        [SwaggerResponse(200, "", typeof(ServiceResult<IList<UserDto>>))]
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
        [Route("{id}")]
        public async Task<ServiceResult> DeleteProjectUser([FromRoute]Guid userId)
        {
            await ProjectUserService.DeleteProjectUser(  userId);
            return await ServiceResult.IsSuccess();
        }
        
    }
}
