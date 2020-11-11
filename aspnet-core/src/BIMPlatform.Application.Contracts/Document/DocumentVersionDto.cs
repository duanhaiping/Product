using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DocumentVersionDto
    {
        public DocumentVersionDto()
        {
            IsLinkDoc = false;
            CanPreview = false;
        }

        public long ID { get; set; }

        public long DocumentID { get; set; }

        public string DocNumber { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 后缀名
        /// </summary>
        public string Suffix { get; set; }
        /// <summary>
        /// MD5值
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string Size { get; set; }

        public string SizeDisplay { get; set; }

        /// <summary>
        /// 版本信息
        /// </summary>
        public int Version { get; set; }

        public string CreationUser { get; set; }

        public int CreationUserID { get; set; }

        public string Status { get; set; }

        public string StatusStr { get; set; }

        /// <summary>
        /// 自定义属性
        /// </summary>
        public string Properties { get; set; }

        public string RemotePath { get; set; }

        public string Base64Stream { get; set; }
        public bool IsLinkDoc { get; set; }
        public string PreviewPath { get; set; }
        public bool CanPreview { get; set; }
        public string Tags { get; set; }
        public long FolderID { get; set; }
        public string FolderName { get; set; }
        public string DocumentComment { get; set; }
        public ImageFileItem ImageFileItem { get; set; }

        public UserDataInfo.UserDto CreationUserInfo { get; set; }
        public string ParentFolderName { get; set; }
    }
}
