﻿using BIMPlatform.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BIMPlatform.Repositories.Document
{
    public class ProjectRootFolderRepository : BaseRepository<BIMPlatform.Document.ProjectRootFolder, long>, IProjectRootFolderRepository
    {
        public ProjectRootFolderRepository(IDbContextProvider<BIMPlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
