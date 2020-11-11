using BIMPlatform.Application.Contracts.ProjectDto;
using System.Collections.Generic;


namespace BIMPlatform.Infrastructure.Project.Services.Interfaces
{
    public interface IProjectConfigurationService
    {
        IList<IDictionary<string, object>> GetDesignChangeCategories(string modeName = "", string systemName = "");
        IDictionary<string, IDictionary<string, object>> GetXMLIssueCategory();
        IList<EntityNamingRule> GetProjectAbb(string projectName, string ruleName = "");
    }
}
