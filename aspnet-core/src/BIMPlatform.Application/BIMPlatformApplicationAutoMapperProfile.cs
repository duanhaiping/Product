using AutoMapper;
using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Application.Contracts.UserDataInfo;
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

            CreateMap<Projects.Project, ProjectDto>();
            CreateMap<ProjectDto, Projects.Project>();
            CreateMap<ProjectCreateParams, Projects.Project>();

            //CreateMap<Document.Document, DocumentUploadParams>();
            //CreateMap<Document.Document, DocumentDto>();
            //CreateMap<DocumentDto, Document.Document>();
            //CreateMap<DocumentUploadParams, Document.Document>();
            //CreateMap<Document.Document, DocumentDto>();
            //CreateMap<DocumentDto, Document.Document>();
            //CreateMap<DocumentFolder, FolderDto>();
            //CreateMap<DocumentVersion, DocumentVersionDto>();
            CreateMap<AppUser, UserDto>();
            CreateMap<UserDto,AppUser >();
          
        }
    }
}
