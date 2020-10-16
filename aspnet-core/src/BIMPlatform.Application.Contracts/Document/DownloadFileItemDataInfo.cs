using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DownloadFileItemDataInfo
    {
        public string Name { get; set; }
        /// <summary>
        ///  The full path of the downloaded file in document management system
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The relative file path of the downloaded file, starts from the website root folder
        /// </summary>
        public string RelativePath { get; set; }
    }
}
