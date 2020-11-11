using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class DocumentDto : AuditedEntityDto<Guid>
    {
        public string DocNumber;

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
        public List<DocumentVersionDto> Versions { get; set; }


        private List<AtuoNumberKeyVaribale> ProvideKeyVariable { get; set; }
        public DocumentVersionDto GetLatestVersion()
        {
            return Versions.OrderByDescending(ver => ver.Version).FirstOrDefault();
        }
        public DocumentVersionDto CreateNextVersion(long targetFolderID, DocumentFileDataInfo fileInfo, UserDataInfo.UserDto creationUserInfo)
        {
            DocumentVersionDto currentVersion = GetLatestVersion();
            DocumentVersionDto nextVersion = new DocumentVersionDto();
            //nextVersion.ParentDocument = this;
            if (currentVersion != null)
            {
                nextVersion.Version = currentVersion.Version + 1;
            }
            else
            {
                nextVersion.Version = 1;
            }

            nextVersion.FolderID = targetFolderID;
            nextVersion.Name = fileInfo.Name;
            nextVersion.Suffix = fileInfo.Suffix;
            //nextVersion.MD5 = GetFileMD5.GetMD5HashFromFile(fileInfo.Stream);
            //nextVersion.FileLength = fileInfo.Stream.Length;
            nextVersion.Properties = XmlOption.GetXmlProperty(fileInfo.CustomizedProperties);
            nextVersion.CreationUserInfo = creationUserInfo;
            nextVersion.CreationDate = DateTime.Now;
            nextVersion.ParentFolderName = DateTime.Now.ToString("yyyy_M_d");
            nextVersion.RemotePath = Path.Combine(nextVersion.ParentFolderName, Guid.NewGuid().ToString());
            nextVersion.Status = string.IsNullOrEmpty(fileInfo.Status) ? "Created" : fileInfo.Status;

            return nextVersion;
        }
        public string GetAutoId(string result)
        {
            foreach (var variable in ProvideKeyVariable)
            {
                result = result.Replace(variable.Key, variable.KeyValue);
            }
            return result;
        }
        public class AtuoNumberKeyVaribale
        {
            public string KeyName { get; set; }
            public string Key { get; set; }
            public string KeyValue { get; set; }
            public bool IsSystem { get; set; }
        }
        public static class KeyValue
        {
            public static string ProjectName = "{Project}";
            public static string Sequence = "{Sequence}";
            public static string Folder = "{Folder}";
        }
        public static class KeyValueName
        {
            public static string ProjectName = "Project";
            public static string Sequence = "Sequence";
            public static string Folder = "Folder";
        }
        public void SetDefaultProvideKeyVariableValue(string projectName, string folderName, int? maxNumber)
        {
            foreach (var item in ProvideKeyVariable)
            {
                if (item.Key == KeyValue.ProjectName)
                    item.KeyValue = projectName;
                if (maxNumber != null)
                {
                    if (item.Key == KeyValue.Sequence)
                        item.KeyValue = (maxNumber + 1).ToString().PadLeft(3, '0');
                }

                if (item.Key == KeyValue.Folder)
                    item.KeyValue = folderName;
            }
        }
    }
}
