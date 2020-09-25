using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Application.Contracts.ProjectDataInfo;
using BIMPlatform.Document;
using BIMPlatform.ProjectDataInfo;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace BIMPlatform.DocumentService
{
    public partial interface IDocumentService
    {
        Task<DocumentVersion> UploadFile(int projectID, DocumentFileDataInfo document);
        Task<IList<DocumentVersion>> GetLatestDocVersionsByFolderInternal(long folderID, string suffix);
        Task<IList<string>> DeleteDocumentsOfFolderInternal(int projectID, Guid userID, long folderID, bool requireRecycle, Guid recycleIdentity);
        DownloadFileItemDataInfo DownloadFiles(IList<long> versionIDs, Guid userId);
        void DeleteDocumentByVersionID(int projectID, Guid userID, long versionID, bool requireRecycle, Guid recycleIdentity);
        Task CopyDocumentsToFolderAsync(int projectID, long targetFolderID, List<long> documentIDs, Guid userID);
        Task MoveDocumentsToFolderAsync(int projectID, long targetFolderID, List<long> documentIDs, Guid userID);
    }
}
