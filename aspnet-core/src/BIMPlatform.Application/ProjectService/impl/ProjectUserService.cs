using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.ProjectDto;
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
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, projectRepository)
        {
            UserRepository = userRepository;
            ProjectUserRepository = projectUserRepository;
            DataFilter = dataFilter;
            ProjectRepository = projectRepository;
        }
        public async Task AddUserToProject(List<Guid> userIdList)
        {
           
            string[] include = new string[] { "Project", "User" };
            var existed = ProjectUserRepository.FindList(c=>c.Project== CurrentProject, include).Select(c=>c.User.Id);
           
            IList<ProjectUser> projectUsers = new List<ProjectUser>();
            foreach (var item in userIdList)
            {
                if (!existed.Contains(item))
                {
                    ProjectUser temp = new ProjectUser
                    {
                        Project = CurrentProject,
                        User = UserRepository.FindByKeyValues(item)
                    };
                    await ProjectUserRepository.InsertAsync(temp);

                }
            }
           

        }

        public async Task DeleteProjectUser( Guid userId)
        {
            await ProjectUserRepository.DeleteAsync(c => c.Project == CurrentProject && c.User.Id == userId);
        }

        public async Task<PagedResultDto<UserDto>> GetProjectUserList(BasePagedAndSortedResultRequestDto filter)
        {
            PagedResultDto<UserDto> result;
            var userid = ProjectUserRepository.Query(c =>c.Project == CurrentProject).Select(c=>c.User.Id);
            var target = UserRepository.Query(c => userid.Contains(c.Id) && c.UserName.Contains(filter.Filter));
            target = !string.IsNullOrEmpty(filter.Sorting) ? target.OrderBy(c => filter.Sorting) : target.OrderByDescending(t => t.CreationTime);
            var count = target.Count();
            var projectList = target.Skip(filter.SkipCount).Take(filter.MaxResultCount).ToList();
            result = new PagedResultDto<UserDto>(count, ObjectMapper.Map<IList<AppUser>, List<UserDto>>(projectList));
            return result;
        }
        public async Task<PagedResultDto<ProjectDto>> GetUserProjectList(BasePagedAndSortedResultRequestDto filter)
        {
            PagedResultDto<ProjectDto> result;
            var currentUser = UserRepository.FindByKeyValues(CurrentUser.Id);
            string[] include = new string[] { "Project", "User" };
            var project= ProjectUserRepository.
                Query(c => c.User == currentUser, include).
                WhereIf(!filter.Filter.IsNullOrWhiteSpace(), c => c.Project.Name.Contains(filter.Filter)).Select(c=>c.Project);
            // 排序
            project = !string.IsNullOrEmpty(filter.Sorting) ? project.OrderBy(c => filter.Sorting) : project.OrderByDescending(t => t.CreationTime);
            var count = project.Count();
            var projectList = project.Skip(filter.SkipCount).Take(filter.MaxResultCount).ToList();
            result = new PagedResultDto<ProjectDto>(count, ObjectMapper.Map<IList<Projects.Project>, List<ProjectDto>>(projectList));
            return result;
        }
        public async Task<PagedResultDto<UserDto>> GetTenantUserForAddToProject(BasePagedAndSortedResultRequestDto filter)
        {
            PagedResultDto<UserDto> result;

            using (DataFilter.Enable<ISoftDelete>())
            {
                string[] include = new string[] { "Project", "User" };
                var projectUsers =ProjectUserRepository.FindList(c=>c.Project==CurrentProject, include). Select(c => c.User.Id).ToList();
              
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
