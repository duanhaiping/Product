using BIMPlatform.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BIMPlatform.Repositories.Document
{
    public class DocumentVersionRepository : BaseRepository<BIMPlatform.Document.DocumentVersion, long>, IDocumentVersionRepository
    {
        public DocumentVersionRepository(IDbContextProvider<BIMPlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
