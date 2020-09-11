using AutoMapper;

namespace BIMPlatform
{
    public class BIMPlatformApplicationAutoMapperProfile : Profile
    {
        public BIMPlatformApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Projects.Project, ProjectDataInfo.ProjectDto>();
            CreateMap<ProjectDataInfo.ProjectDto, Projects.Project>();
            CreateMap<ProjectDataInfo.ProjectCreateParams, Projects.Project>();

            CreateMap<Document.Document, Application.Contracts.DocumentDataInfo.DocumentUploadParams>();

            CreateMap<Application.Contracts.DocumentDataInfo.DocumentUploadParams, Document.Document>();
            CreateMap<Document.Document, Application.Contracts.DocumentDataInfo.DocumentDto>();
            CreateMap<Application.Contracts.DocumentDataInfo.DocumentDto, Document.Document>();
        }
    }
}
