using AutoMapper;
using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Application.Contracts.UserDataInfo;
using BIMPlatform.Document;
using BIMPlatform.Users;

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

            CreateMap<Document.Document, DocumentUploadParams>();
            CreateMap<Document.Document, Application.Contracts.DocumentDataInfo.Domain.Document>();
            CreateMap<Application.Contracts.DocumentDataInfo.Domain.Document, Document.Document>();
            CreateMap<DocumentUploadParams, Document.Document>();
            CreateMap<Document.Document, DocumentDto>();
            CreateMap<DocumentDto, Document.Document>();
            CreateMap<DocumentFolder, FolderDto>();
            CreateMap<DocumentVersion, Application.Contracts.DocumentDataInfo.Domain.DocumentVersion>();
            CreateMap<AppUser, UserDto>();
            CreateMap<UserDto,AppUser >();
        }
    }
}
