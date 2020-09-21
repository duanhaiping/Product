using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.DocumentService
{
    public partial interface IDocumentFolderCommonService
    {
        long CreateFolder(long? parentFolderID, string folderName);
    }
}
