using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.DocumentService
{
    public partial interface IDocumentFolderService
    {
        Task<List<DocumentFolderDto>> GetAllFolders(long rootFolderID);
        long CreateFolder(long? parentFolderID, string folderName);
        bool RenameFolderName(long folderId, string newName);
        bool DeleteFolder(int projectID, int userID, long folderID, bool requireRecycle, Guid recycleIdentity);
    }
}
