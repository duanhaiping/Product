using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using BIMPlatform.ProjectService;
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
    [Route("api/project")]
    /// <summary>
    /// 项目管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGrouping.GroupName_v4)]
    public class ProjectController : BaseController
    {
        private IProjectService ProjectService { get; set; }

      
        public ProjectController(IProjectService projectService,IProjectUserService projectUserService)
        {
            ProjectService = projectService;
            
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(200, "", typeof(ServiceResult<ProjectDto>))]
        public async Task<ServiceResult> GetProject([FromRoute]int id)
        {
            var project = await ProjectService.GetProjectAsync(id);
            return await ServiceResult<ProjectDto>.IsSuccess(project);
        }
        /// <summary>
        /// 获取当前租户下的所有项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, "", typeof(ServiceResult<IList<ProjectDto>>))]
        [Route("page")]
        public async Task<ServiceResult> GetProjects([FromQuery]BasePagedAndSortedResultRequestDto filter)
        {
            var project = await ProjectService.GetProjects(filter);
            return await ServiceResult<IList<ProjectDto>>.PageList(project.Items.ToList(), project.TotalCount, string.Empty);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="createParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResult> Create([FromBody]ProjectCreateParams createParams)
        {
            await ProjectService.CreateAsync(createParams);
            return await ServiceResult.IsSuccess();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="updateParams"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ServiceResult> Update([FromBody]ProjectUpdateParams updateParams)
        {
            await ProjectService.UpdateAsync(updateParams);
            return await ServiceResult.IsSuccess();
        }
        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ServiceResult> Delete([FromRoute]int id)
        {

            await ProjectService.DeleteAsync(id);
            return await ServiceResult.IsSuccess();
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        [SwaggerResponse(200, "", typeof(ServiceResult<IList<ProjectDto>>))]
        public async Task<ServiceResult> GetProjectList()
        {
            var project = await ProjectService.GetProjects();
            return await ServiceResult<IList<ProjectDto>>.IsSuccess(project, string.Empty);
        }
    }
}
