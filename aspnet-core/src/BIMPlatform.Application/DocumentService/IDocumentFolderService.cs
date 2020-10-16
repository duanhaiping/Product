using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Document;
using BIMPlatform.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.DocumentService
{
    public partial interface IDocumentFolderService
    {
        Task<List<FolderStructure>> GetAllFolders(long rootFolderID);
        long CreateFolder(long? parentFolderID, string folderName);
        bool RenameFolderName(long folderId, string newName);
        bool DeleteFolder(int projectID, Guid userID, long folderID, bool requireRecycle, Guid recycleIdentity);
        FolderDto GetProjectRootFolder(int projectID);
        List<FolderStructure> GetFolderStructure(long rootFolderID, string suffix, Guid userID , bool getDocCount = false);
    }
}
