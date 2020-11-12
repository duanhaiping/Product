using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.ToolKits.Base;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Controllers.Identity
{
    [Area("identity")]
    [ControllerName("UserLookup")]
    [Route("api/identity/users/lookup")]
    [ApiExplorerSettings(GroupName = ApiGrouping.GroupName_v1)]
    public class BIMIdentityUserController  : BaseController
    {
        protected IProjectUserService ProjectUserService { get; set; }

        public BIMIdentityUserController(IProjectUserService roleAppService)
        {
            ProjectUserService = roleAppService;
        }
        /// <summary>
        /// 获取用户的项目列表
        /// </summary>
        /// <param name="filter">BasePagedAndSortedResultRequestDto </param>
        /// <returns>ProjectDto </returns>
        [Route("projects")]
        [HttpGet]
        [SwaggerResponse(200, "", typeof(ServiceResult<IList<ProjectDto>>))]
        public async Task<ServiceResult> GetUserProjectInfo([FromQuery]BasePagedAndSortedResultRequestDto filter)
        {
            var project = await ProjectUserService.GetUserProjectList(filter);
            return await ServiceResult<IList<ProjectDto>>.PageList(project.Items.ToList(), project.TotalCount, string.Empty);
        }
    }
}
