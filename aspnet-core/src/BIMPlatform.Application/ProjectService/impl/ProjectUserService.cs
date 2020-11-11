using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.UserDataInfo;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using BIMPlatform.Project.Repositories;
using BIMPlatform.Projects;
using BIMPlatform.Projects.Repositories;
using BIMPlatform.Users;
using BIMPlatform.Users.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace BIMPlatform.Module.Project.Services.Default
{
    public class ProjectUserService : BaseService, IProjectUserService
    {
        private readonly IProjectRepository ProjectRepository;

        private readonly IProjectUserRepository ProjectUserRepository;
        private readonly IDataFilter DataFilter;
        private readonly IUserRepository UserRepository;
        public ProjectUserService(
            IProjectRepository projectRepository, 
            IDataFilter dataFilter,
            IUserRepository userRepository,
            IProjectUserRepository projectUserRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            UserRepository = userRepository;
            ProjectUserRepository = projectUserRepository;
            DataFilter = dataFilter;
            ProjectRepository = projectRepository;
        }
        public async Task AddUserToProject(List<Guid> userIdList)
        {
            var existed = ProjectUserRepository.FindList(c=>c.Project.Id==CurrentProject).Select(c=>c.UserId);
            var project =await ProjectRepository.FindAsync(c => c.Id == CurrentProject);
            IList<ProjectUser> projectUsers = new List<ProjectUser>();
            foreach (var item in userIdList)
            {
                if (!existed.Contains(item))
                {
                    ProjectUser temp = new ProjectUser
                    {
                        Project = project,
                        UserId = (await UserRepository.FindAsync(item)).Id
                    };
                    ProjectUserRepository.InsertAsync(temp);
                   
                }
            }
            //ProjectUserRepository.InsertAsync(projectUsers);

        }

        public async Task DeleteProjectUser( Guid userId)
        {
            await ProjectUserRepository.DeleteAsync(c => c.Project.Id == CurrentProject && c.UserId == userId);
        }

        public async Task<PagedResultDto<UserDto>> GetProjectUserList(BasePagedAndSortedResultRequestDto filter)
        {
            PagedResultDto<UserDto> result;
            var userid = ProjectUserRepository.Query(c =>c.Project.Id== CurrentProject).Select(c=>c.UserId);
            var target = UserRepository.Query(c => userid.Contains(c.Id) && c.UserName.Contains(filter.Filter));
            target = !string.IsNullOrEmpty(filter.Sorting) ? target.OrderBy(c => filter.Sorting) : target.OrderByDescending(t => t.CreationTime);
            var count = target.Count();
            var projectList = target.Skip(filter.SkipCount).Take(filter.MaxResultCount).ToList();
            result = new PagedResultDto<UserDto>(count, ObjectMapper.Map<IList<AppUser>, List<UserDto>>(projectList));
            return result;
        }

        public async Task<PagedResultDto<UserDto>> GetTenantUserForAddToProject(BasePagedAndSortedResultRequestDto filter)
        {
            PagedResultDto<UserDto> result;

            using (DataFilter.Enable<ISoftDelete>())
            {
                var projectUsers =(await ProjectUserRepository.GetListAsync()).WhereIf(CurrentProject > 0, c => c.Project.Id == CurrentProject). Select(c => c.UserId).ToList();
                var target = (await UserRepository.GetListAsync())
                    .WhereIf(!filter.Filter.IsNullOrWhiteSpace(), t => t.Name.Contains(filter.Filter))
                    .WhereIf(true,c=>c.IsActivated==true)
                    .WhereIf(!(projectUsers==null),t=> projectUsers.Contains(t.Id)==false);

                var count = target.Count();
                var projectList = target.Skip(filter.SkipCount).Take(filter.MaxResultCount).ToList();
                result = new PagedResultDto<UserDto>(count, ObjectMapper.Map<IList<AppUser>, List<UserDto>>(projectList));
            }

            return result;
        }
    }
}
