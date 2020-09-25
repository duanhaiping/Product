using System;
using System.Collections.Generic;
using System.Text;
using BIMPlatform.Application.Contracts.Entity;
using BIMPlatform.Application.Contracts.Events;
using BIMPlatform.Localization;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;

namespace BIMPlatform
{
    /* Inherit your application services from this class.
     */
    public abstract class BaseService : ApplicationService
    {
        //public IEventService EventService { get; private set; }
        public int CurrentProject;
        protected IHttpContextAccessor httpContext;
        protected BaseService(IHttpContextAccessor  httpContextAccessor)
        {
            httpContext = httpContextAccessor;
            CurrentProject = httpContextAccessor.HttpContext.Request.Headers.ContainsKey("__currentproject")? int.Parse( httpContextAccessor.HttpContext.Request.Headers["__currentproject"]) :0;
            LocalizationResource = typeof(BIMPlatformResource);
        }
       
        public virtual void SubscribeEvent(EntityDataInfo entityInfo, int userID, string eventSystemName, NotificationType type)
        {
            #region Todo
            //    if (EventService != null)
            //    {
            //        EventService.SubscribeEvent(entityInfo, eventSystemName, userID, type);
            //    }
            #endregion
        }
    }
}
