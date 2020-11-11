using BIMPlatform.EntityFrameworkCore;
using BIMPlatform.Projects;
using BIMPlatform.Projects.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore;

namespace BIMPlatform.Repositories.Project
{
    

    public class UserInProjectRepository : BaseRepository<UserInProject, long>, IUserInProjectRepository
    {
        public UserInProjectRepository(IDbContextProvider<BIMPlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
