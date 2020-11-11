using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Document;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        List<DocumentVersionDto> GetProjectImgDocumentVersions(int projectID);
        string GetPreviewFileRelativePath(DocumentVersionDto docVersion);
        DocumentVersionDto GetProjectCoverImgDocumentVersions(int projectID);
    }
}
