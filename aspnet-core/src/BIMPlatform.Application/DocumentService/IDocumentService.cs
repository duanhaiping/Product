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
        Task<IList<string>> DeleteDocumentsOfFolderInternal(int projectID, int userID, long folderID, bool requireRecycle, Guid recycleIdentity);
        DownloadFileItemDataInfo DownloadFiles(IList<long> versionIDs, int userId);
        void DeleteDocumentByVersionID(int projectID, int userID, long versionID, bool requireRecycle, Guid recycleIdentity);
        void CopyDocumentsToFolder(int projectID, long targetFolderID, List<long> documentIDs, int userID);
        void MoveDocumentsToFolder(int projectID, long targetFolderID, List<long> documentIDs, int userID);
    }
}
