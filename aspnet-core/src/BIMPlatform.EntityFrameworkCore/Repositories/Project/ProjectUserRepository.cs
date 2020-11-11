using BIMPlatform.EntityFrameworkCore;
using BIMPlatform.Projects.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore;

namespace BIMPlatform.Repositories.Project
{
    public class ProjectUserRepository : BaseRepository<Projects.ProjectUser, int>, IProjectUserRepository
    {
        public ProjectUserRepository(IDbContextProvider<BIMPlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
