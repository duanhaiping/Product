using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DocumentDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// The ID of the current upload session for multiple files, optional
        /// assigned value for form + document
        /// </summary>
        public string BatchUploadID { get; set; }

        /// <summary>
        /// The ID of the current upload sessio for a single File, must be assigned value
        /// </summary>
        public string UploadID { get; set; }

        /// <summary>
        /// The file uploaded into temp folder
        /// </summary>
        public DateTime UploadedTime { get; set; }

        /// <summary>
        /// The full temporary file path when uploading
        /// </summary>
        public string UploadTempPath { get; set; }

        /// <summary>
        /// Relative path of doc root folder
        /// </summary>
        public string UploadTempRelativePath { get; set; }

        public string SizeDisplay { get; set; }

        /// <summary>
        ///  Current Selected Folder, 
        ///  maybe not the file uploaded target folder, depends on ClientRelativePath
        /// </summary>
        public long FolderID { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// File Stream
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Document creation user id
        /// </summary>
        public int CreationUserID { get; set; }

        //public List<Customized> CustomizedProperties { get; set; }

        public bool SupportDocNumber { get; set; }

        public bool RequireTransform { get; set; }

        public int ProjectID { get; set; }

        public List<int> NotificationUserId { get; set; }

        public string Tags { get; set; }

        public string Status { get; set; }

        public string ClientRelativePath { get; set; }

        public string VerifiedPermissionName { get; set; }

        public string Description { get; set; }
    }
}
