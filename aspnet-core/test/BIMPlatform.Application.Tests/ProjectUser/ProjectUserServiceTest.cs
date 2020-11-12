using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Xunit;
using System.Threading.Tasks;
using BIMPlatform.Project.Repositories;

namespace BIMPlatform.ProjectUser
{
    public class ProjectUserServiceTest : BIMPlatformApplicationTestBase
    {
        private readonly IProjectUserService _projectUserService;
        private readonly IProjectRepository _projectRepository;
        public ProjectUserServiceTest()
        {
            _projectUserService = GetRequiredService<IProjectUserService>();
            _projectRepository = GetRequiredService<IProjectRepository>();
        }

        [Fact]
        public async Task Initial_Data_Should_Contain_Admin_User()
        {
            //Act
            var result = await _projectRepository.GetListAsync(true);

            //Assert
           
        }
    }
}
