using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public enum OperationRecordType
    {
        DocumentUpload,
        RecycleDocument,
        DeleteDocumentVersion,
        CleanDocument,
        DownloadDocument
    }
}
