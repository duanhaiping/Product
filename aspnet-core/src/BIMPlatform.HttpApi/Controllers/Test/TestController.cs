using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.ToolKits.Base;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Controllers.Test
{
    //[AllowAnonymous]
    public class TestController : BaseController
    {
        public TestService.TestService TestService { get; set; }
        public TestController(TestService.TestService testService)
        {
            TestService = testService;
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
    }
}
