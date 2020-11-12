using BIMPlatform.Application.Contracts.Identity;
using BIMPlatform.ToolKits.Enums;
using BIMPlatform.ToolKits.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.ToolKits.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace BIMPlatform.Controllers
{
    [Area("identity")]
    [ControllerName("Role")]
    [Route("api/identity/roles")]

    [ApiExplorerSettings(GroupName = ApiGrouping.GroupName_v1)]
    public class BIMIdentityRoleController : BaseController
    {
        protected IdentityRoleAppService RoleAppService { get; }
        public BIMIdentityRoleController(IdentityRoleAppService roleAppService)
        {
            RoleAppService = roleAppService;
        }
        /// <summary>
        ///  新增角色 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bimCreate")]
        [SwaggerResponse(200, "", typeof(ServiceResult<IdentityRoleDto>))]
        public async Task<ServiceResult> BIMCreate([FromBody]BIMIdentityRoleCreateDto input)
        {
            IdentityRoleCreateDto createDto = new IdentityRoleCreateDto
            {
                Name = input.Name,
                IsDefault = input.IsDefault,
                IsPublic = input.IsPublic
            };
            createDto.SetProperty("RoleType", EnumHelper.GetEnumName<RoleTypeEnum>(input.RoleType));
            IdentityRoleDto result = await RoleAppService.CreateAsync(createDto);

            return await ServiceResult<IdentityRoleDto>.IsSuccess(result);
        }
    }
}
