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
        /// <summary>
        ///  所属文件夹ID(原框架定义)
        /// </summary>
        [Required]
        public long CurrentNode { get; set; }

        public string FileName { get; set; }

        public string Part { get; set; }

        public string BatchUploadID { get; set; }

        public string Guid { get; set; }

        public int Size { get; set; }
    }

    public enum StatusEnum
    {
        Create,
        Update,
        Recycled
    }
}
