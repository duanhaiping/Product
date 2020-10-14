using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.TestService
{
    public class TestService : BaseService
    {
        public TestService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        public string TestLanguage()
        {
            return L["ProjectError:NameDuplicate"];
        }

        public int TestCurrentProject()
        {
            return CurrentProject;
        }
    }
}
