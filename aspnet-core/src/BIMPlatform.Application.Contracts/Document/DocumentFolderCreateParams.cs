using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DocumentFolderCreateParams
    {
        public long? FolderId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
