using BIMPlatform.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.Document
{
    public class CommentedDocumentVersion
    {
        public DocumentVersion DocVersion { get; set; }
        public string Description { get; set; }
    }
}
