using BIMPlatform.Application.Contracts.DocumentDataInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.Entity
{
    public class AssociationEntityDataInfo : EntityDataInfo
    {
        public int UserID { get; set; }
        public string Description { get; set; }
        public DocumentVersionDto DocVersion { get; set; }
    }
}
