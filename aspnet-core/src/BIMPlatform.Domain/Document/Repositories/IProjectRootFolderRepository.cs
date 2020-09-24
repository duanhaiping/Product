using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories;

namespace BIMPlatform.Repositories.Document
{
    public interface IProjectRootFolderRepository : IBaseRepository<BIMPlatform.Document.ProjectRootFolder, long>
    {
    }
}
