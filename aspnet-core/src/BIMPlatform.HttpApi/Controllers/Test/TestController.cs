using Microsoft.AspNetCore.Mvc;
using Platform.ToolKits.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace BIMPlatform.Controllers.Test
{
    //[AllowAnonymous]
    public class TestController : BaseController
    {
        public TestService.TestService TestService { get;  }
        //protected BIMIdentityRoleAppService RoleAppService { get; }

        public TestController(TestService.TestService testService /*, BIMIdentityRoleAppService bIMIdentityRoleAppService*/)
        {
            TestService = testService;
            //RoleAppService = bIMIdentityRoleAppService;
        }
        [HttpGet]
        [SwaggerResponse(200, "", typeof(ServiceResult))]
        public  Task<ServiceResult> TestLanguage()
        {
            var msg = TestService.TestLanguage();
            return  ServiceResult.IsSuccess(msg);
        }

        [HttpGet]
        [SwaggerResponse(200, "", typeof(ServiceResult))]
        public Task<ServiceResult> TestCurrentProject()
        {
            var msg = TestService.TestCurrentProject();
            return ServiceResult.IsSuccess(msg.ToString());
        }

        //[HttpPut]
        //[Route("{id}")]
        //public virtual Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
        //{
        //    return RoleAppService.UpdateAsync(id, input);
        //}
    }
}
