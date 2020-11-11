using BIMPlatform.Application.Contracts.DocumentDataInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.Document
{
    public class CommentedDocumentFileDataInfo : DocumentFileDataInfo
    {
        public string Description { get; set; }
    }
}
