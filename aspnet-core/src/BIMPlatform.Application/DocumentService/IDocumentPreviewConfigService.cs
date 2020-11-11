using BIMPlatform.Application.Contracts.DocumentDataInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.DocumentService
{
    public interface IDocumentPreviewConfigService
    {
        IList<DocumentPreviewConfigItem> GetPreviewConfiguration();
        DocumentPreviewConfigItem GetPreviewConfigBySuffix(string suffix);
    }
}
