using BIMPlatform.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.DocumentService
{
    public partial interface IDocumentFolderCommonService
    {
        long CreateFolder(long? parentFolderID, string folderName);
        DocumentFolder GetOrCreateFolderByPathInternal(DocumentFolder parentFolder, int creationUserID, string folderPath);
        bool CanCopyDocuments(long targetFolderID, List<long> documentIDs, out string error);
        bool CanMoveDocuments(long targetFolderID, List<long> documentIDs, out string error);
    }
}
