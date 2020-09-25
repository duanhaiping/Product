using BIMPlatform.EntityFrameworkCore;
using BIMPlatform.Users;
using BIMPlatform.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore;

namespace BIMPlatform.Repositories.User
{
    public class UserRepository : BaseRepository<AppUser, Guid>, IUserRepository
    {
        public UserRepository(IDbContextProvider<BIMPlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
