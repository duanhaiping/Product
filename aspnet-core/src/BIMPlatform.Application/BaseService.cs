using BIMPlatform.Localization;
using BIMPlatform.Project.Repositories;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;

namespace BIMPlatform
{
    /* Inherit your application services from this class.
     */
    public abstract class BaseService : ApplicationService
    {
        //public IEventService EventService { get; private set; }
        public Projects.Project CurrentProject;
        protected IHttpContextAccessor httpContext;
        private readonly IProjectRepository ProjectRepository;

        protected BaseService(IHttpContextAccessor  httpContextAccessor, IProjectRepository projectRepository)
        {
            httpContext = httpContextAccessor;
            ProjectRepository = projectRepository;
            int currentProjectId = 0;
            if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("__currentproject")) {
                int.TryParse(httpContextAccessor.HttpContext.Request.Headers["__currentproject"], out  currentProjectId);
                CurrentProject = ProjectRepository.FindByKeyValues(currentProjectId);
            }
            LocalizationResource = typeof(BIMPlatformResource);
        }
       
        //public virtual void SubscribeEvent(EntityDataInfo entityInfo, int userID, string eventSystemName, NotificationType type)
        //{
        //    #region Todo
        //    //    if (EventService != null)
        //    //    {
        //    //        EventService.SubscribeEvent(entityInfo, eventSystemName, userID, type);
        //    //    }
        //    #endregion
        //}
    }
}
