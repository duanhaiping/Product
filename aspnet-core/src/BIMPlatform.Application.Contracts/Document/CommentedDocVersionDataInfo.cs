using BIMPlatform.Application.Contracts.DocumentDataInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.Document
{
    public class CommentedDocVersionDataInfo
    {
        public DocumentVersionDto DocVersionInfo { get; set; }
        public string Description { get; set; }
    }
}
