﻿using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Project.Repositories;
using BIMPlatform.Projects;
using BIMPlatform.Projects.Repositories;
using BIMPlatform.Users.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace BIMPlatform.ProjectService.impl
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IProjectRepository ProjectRepository;

        private readonly IProjectUserRepository ProjectUserRepository;
        private readonly IUserRepository UserRepository;
        private readonly IDataFilter DataFilter;
        public ProjectService(
            IProjectRepository projectRepository,
            IProjectUserRepository projectUsersRepository,
            IUserRepository usreRepository,
            IDataFilter dataFilter,
            IHttpContextAccessor httpContextAccessor) :
            base(httpContextAccessor, projectRepository)
        {
            UserRepository = usreRepository;
            DataFilter = dataFilter;
            ProjectRepository = projectRepository;
            ProjectUserRepository = projectUsersRepository;
        }
        public async Task CreateAsync(ProjectCreateParams projectDto)
        {
            Projects.Project project = ObjectMapper.Map<ProjectCreateParams, Projects.Project>(projectDto);
            project.CreationTime = DateTime.Now;
            project.CreatorId = CurrentUser.Id;
            project.TenantId = CurrentTenant.Id;
            project.IsDeleted = false;
            
            var existed=  ProjectRepository.FirstOrDefault(c=>c.Name==project.Name );
            if (existed != null)
                throw new ArgumentException(L["ProjectError:NameDuplicate"]);
            var insertResult=await ProjectRepository.InsertAsync(project);
            var principal= UserRepository.FindByKeyValues(project.Principal);
            if(principal == null)
                throw new ArgumentException(string.Format(L["Error:User:NotExisted"], project.Principal) );
            ProjectUser projectUser = new ProjectUser { Project = insertResult, User = principal };
            await ProjectUserRepository.InsertAsync(projectUser);
            // TODO: add default project role 

            // TODO: add Notification To  principal if currentuser is not principal 

            //UnresolvedMergeConflict:s
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int projectID)
        {
            Projects.Project project = await ProjectRepository.FindAsync(c => c.Id==projectID);
            // TODO: should delete project user ??

            //TODO :should Notification  to any Project user 
            await ProjectRepository.DeleteAsync(project);
        }


        public async  Task<ProjectDto> GetProjectAsync(int projectID)
        {
            Projects.Project project = await ProjectRepository.FindAsync(c => c.Id == projectID);
            return ObjectMapper.Map<Projects.Project, ProjectDto>(project);
        }

        public async Task<PagedResultDto<ProjectDto>> GetProjects(BasePagedAndSortedResultRequestDto filter)
        {
            PagedResultDto<ProjectDto> result;
            // 默认开启租户过滤，所以不在展示
            using (DataFilter.Enable<ISoftDelete>())
            {
                var target = (await ProjectRepository.GetListAsync()).
                    WhereIf(!filter.Filter.IsNullOrWhiteSpace(),t=>t.Name.Contains(filter.Filter));
                // 排序
                target = !string.IsNullOrEmpty(filter.Sorting) ? target.OrderBy(c=> filter.Sorting) : target.OrderByDescending(t => t.CreationTime);
                var count = target.Count();
                var projectList= target.Skip(filter.SkipCount).Take(filter.MaxResultCount).ToList();
                result = new PagedResultDto<ProjectDto>(count, ObjectMapper.Map<IList<Projects.Project>, List<ProjectDto>>(projectList));
                return result;
            }
        }
        public async Task<IList<ProjectDto>> GetProjects()
        {
            // 默认开启租户过滤，所以不在展示
            using (DataFilter.Enable<ISoftDelete>())
            {
                var target = (await ProjectRepository.GetListAsync());
                return ObjectMapper.Map<IList<Projects.Project>, List<ProjectDto>>(target); ;
            }
        }
        public async Task<ProjectDto> UpdateAsync(ProjectUpdateParams projectDto)
        {
            Projects.Project project = await ProjectRepository.FindAsync(c=>c.Id==projectDto.ID);
            if (project == null)
                throw new ArgumentException(L["ProjectError:NameDuplicate"]);
            if(!projectDto.Name.IsNullOrEmpty() && !projectDto.Name.Equals(project.Name))
            {
                project.Name = projectDto.Name;
            }
            if (!projectDto.Description.IsNullOrEmpty() && !projectDto.Description.Equals(project.Description))
            {
                project.Description = projectDto.Description;
            }
            if (!projectDto.StartDate.Equals(project.StartDate))
            {
                project.StartDate = projectDto.StartDate;
            }
            if (!projectDto.Address.IsNullOrEmpty() && !projectDto.Address.Equals(project.Address))
            {
                project.Address = projectDto.Address;
            }
            if ( !projectDto.ProjectEstimate.Equals(project.ProjectEstimate))
            {
                project.ProjectEstimate = projectDto.ProjectEstimate;
            }
            if (!projectDto.ConstructionUnit.IsNullOrEmpty() && !projectDto.ConstructionUnit.Equals(project.ConstructionUnit))
            {
                project.ConstructionUnit = projectDto.ConstructionUnit;
            }
            if (!projectDto.MainContractor.IsNullOrEmpty() && !projectDto.MainContractor.Equals(project.MainContractor))
            {
                project.MainContractor = projectDto.MainContractor;
            }

            if (!projectDto.DesignOrganization.IsNullOrEmpty() && !projectDto.DesignOrganization.Equals(project.DesignOrganization))
            {
                project.DesignOrganization = projectDto.DesignOrganization;
            }
            if (!projectDto.SupervisingUnit.IsNullOrEmpty() && !projectDto.SupervisingUnit.Equals(project.SupervisingUnit))
            {
                project.SupervisingUnit = projectDto.SupervisingUnit;
            }
            if (!projectDto.ConsultingUnit.IsNullOrEmpty() && !projectDto.ConsultingUnit.Equals(project.ConsultingUnit))
            {
                project.ConsultingUnit = projectDto.ConsultingUnit;
            }
            if ( !projectDto.Area.Equals(project.Area))
            {
                project.Area = projectDto.Area;
            }
            if ( !projectDto.CompleteDate.Equals(project.CompleteDate))
            {
                project.CompleteDate = projectDto.CompleteDate;
            }
            if ( !projectDto.Longitude.Equals(project.Longitude))
            {
                project.Longitude = projectDto.Longitude;
            }
            if (!projectDto.Latitude.Equals(project.Latitude))
            {
                project.Latitude = projectDto.Latitude;
            }
            if ( !projectDto.Principal.Equals(project.Principal))
            {
                project.Principal = projectDto.Principal;
            }
            // TODO : Notification to new Principal, old  Principal how to notification ?? 
            // TODO : If the new principal is not in the project member list, it is added to the member list

            // TODO : What about the original principal,delete or remove principal role? the pendding issue ??  
            project.LastModificationTime = Clock.Now;
            project.LastModifierId = this.CurrentUser.Id;
           return ObjectMapper.Map<Projects.Project, ProjectDto>(await ProjectRepository.UpdateAsync(project));
        }

        
    }
}
