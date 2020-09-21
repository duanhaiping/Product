using System;
using Volo.Abp.Domain.Repositories;

namespace BIMPlatform.Project.Repositories
{
    public interface IProjectRepository : IBaseRepository<Projects.Project,Guid>
    {
    }
}
