using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DocumentUploadParams
    {
        /// <summary>
        ///  状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        ///  所属文件夹ID
        /// </summary>
        [Required]
        public long FolderID { get; set; }
        /// <summary>
        ///  文件具体信息
        /// </summary>
        [Required]
        public IFormFile File { get; set; }
    }

    public enum StatusEnum
    {
        Create,
        Update,
        Recycled
    }
}
