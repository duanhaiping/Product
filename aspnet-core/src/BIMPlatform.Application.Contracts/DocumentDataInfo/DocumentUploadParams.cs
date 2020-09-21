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
        /// 客户端路径
        /// </summary>
        public string ClientRelativePath { get; set; }
        /// <summary>
        ///  文件具体信息
        /// </summary>
        [Required]
        public List<IFormFile> Files { get; set; }
    }

    public enum StatusEnum
    {
        Create,
        Update,
        Recycled
    }
}
