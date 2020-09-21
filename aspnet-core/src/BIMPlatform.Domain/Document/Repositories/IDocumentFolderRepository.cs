using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories;

namespace BIMPlatform.Repositories.Document
{
    public interface IDocumentFolderRepository : IBaseRepository<BIMPlatform.Document.DocumentFolder, long>
    {
    }
}
