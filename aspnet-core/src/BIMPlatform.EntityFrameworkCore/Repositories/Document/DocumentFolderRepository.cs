using BIMPlatform.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BIMPlatform.Repositories.Document
{
    public class DocumentFolderRepository : EfCoreRepository<BIMPlatformDbContext, BIMPlatform.Document.DocumentFolder, long>, IDocumentFolderRepository
    {
        public DocumentFolderRepository(IDbContextProvider<BIMPlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
