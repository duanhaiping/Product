using BIMPlatform.Project.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.TestService
{
    public class TestService : BaseService
    {
        public TestService(IHttpContextAccessor httpContextAccessor, IProjectRepository projectRepository) 
            : base(httpContextAccessor, projectRepository)
        {

        }
        public string TestLanguage()
        {
            return L["ProjectError:NameDuplicate"];
        }

        public Projects.Project TestCurrentProject()
        {
            return CurrentProject;
        }
    }
}
