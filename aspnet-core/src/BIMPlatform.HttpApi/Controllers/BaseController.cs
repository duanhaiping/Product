using BIMPlatform.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace BIMPlatform.Controllers
{
    /* Inherit your controllers from this class.
     */
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = ApiGrouping.GroupName_v999)]
    [Authorize]
    public abstract class BaseController : AbpController
    {
        //public int CurrentUser { get; set; }
    }
}