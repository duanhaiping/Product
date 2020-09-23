using AutoMapper;
using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Document;

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
            CreateMap<DocumentUploadParams, Document.Document>();
            CreateMap<Document.Document, DocumentDto>();
            CreateMap<DocumentDto, Document.Document>();
            CreateMap<DocumentFolder, FolderDataInfo>();
        }
    }
}
