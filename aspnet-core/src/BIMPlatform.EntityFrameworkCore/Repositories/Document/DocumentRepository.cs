using BIMPlatform.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BIMPlatform.Repositories.Document
{
    public class DocumentRepository : EfCoreRepository<BIMPlatformDbContext, BIMPlatform.Document.Document, Guid>, IDocumentRepository
    {
        public DocumentRepository(IDbContextProvider<BIMPlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
