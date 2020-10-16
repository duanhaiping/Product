using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DocumentPreviewConfigItem
    {
        public string Extension { get; set; }
        public string TransformHandler { get; set; }
        public string TargetType { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        public IList<DocumentPreviewConfigItem> SubItems { get; set; }

        public DocumentPreviewConfigItem()
        {
            SubItems = new List<DocumentPreviewConfigItem>();
        }
    }

}
