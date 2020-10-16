using BIMPlatform.Application.Contracts.UserDataInfo;
using BIMPlatform.Module.Document.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo.Domain
{
    public class Document
    {
        public long ID { get; set; }
        public long FolderID { get; set; }
        public string Name { get; set; }
        public string Suffix { get; set; }
        public string DocNumber { get; set; }
        public string Status { get; set; }
        public List<DocumentVersion> Versions { get; set; }

        public Document()
        {
            Versions = new List<DocumentVersion>();
            DefaultSetKeyVariable();
        }

        public DocumentVersion GetLatestVersion()
        {
            return Versions.OrderByDescending(ver => ver.Version).FirstOrDefault();
        }

        public DocumentVersion CreateNextVersion(long targetFolderID, DocumentFileDataInfo fileInfo, UserDataInfo.UserDto creationUserInfo)
        {
            DocumentVersion currentVersion = GetLatestVersion();
            DocumentVersion nextVersion = new DocumentVersion();
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
            nextVersion.FileLength = fileInfo.Stream.Length;
            nextVersion.Properties = XmlOption.GetXmlProperty(fileInfo.CustomizedProperties);
            nextVersion.CreationUserInfo = creationUserInfo;
            nextVersion.CreationDate = DateTime.Now;
            nextVersion.ParentFolderName = DateTime.Now.ToString("yyyy_M_d");
            nextVersion.RemotePath = Path.Combine(nextVersion.ParentFolderName, Guid.NewGuid().ToString());
            nextVersion.Status = string.IsNullOrEmpty(fileInfo.Status) ? "Created" : fileInfo.Status;

            return nextVersion;
        }

        public List<DocumentVersion> GetAllVersions()
        {
            return Versions.OrderByDescending(ver => ver.Version).ToList();
        }

        public override bool Equals(object obj)
        {
            if (obj is Document)
            {
                return ((Document)obj).ID == this.ID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
        public List<AtuoNumberKeyVaribale> ProvideKeyVariable { get; set; }

        public class AtuoNumberKeyVaribale
        {
            public string KeyName { get; set; }
            public string Key { get; set; }
            public string KeyValue { get; set; }
            public bool IsSystem { get; set; }
        }

        public void SetDefaultProvideKeyVariableValue(string projectName, string folderName, int? maxNumber)
        {
            foreach (var item in ProvideKeyVariable)
            {
                if (item.Key == KeyValue.ProjectName)
                    item.KeyValue = projectName;
                if (maxNumber != null)
                {
                    //if (item.Key == KeyValue.Sequence)
                    //    item.KeyValue = (maxNumber + 1).ToStr().PadLeft(3, '0');
                }

                if (item.Key == KeyValue.Folder)
                    item.KeyValue = folderName;
            }
        }
        private void DefaultSetKeyVariable()
        {
            if (ProvideKeyVariable == null)
                ProvideKeyVariable = new List<AtuoNumberKeyVaribale>();
            ProvideKeyVariable.Add(new AtuoNumberKeyVaribale { KeyName = KeyValueName.Sequence, Key = KeyValue.Sequence, KeyValue = "{Sequence}", IsSystem = true });
            ProvideKeyVariable.Add(new AtuoNumberKeyVaribale { KeyName = KeyValueName.ProjectName, Key = KeyValue.ProjectName, KeyValue = "", IsSystem = true });
            ProvideKeyVariable.Add(new AtuoNumberKeyVaribale { KeyName = KeyValueName.Folder, Key = KeyValue.Folder, KeyValue = "", IsSystem = true });
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

        public string GetAutoId(string result)
        {
            foreach (var variable in ProvideKeyVariable)
            {
                result = result.Replace(variable.Key, variable.KeyValue);
            }
            return result;
        }
    }
}
