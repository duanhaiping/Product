using BIMPlatform.Configurations;
using BIMPlatform.Document;
using BIMPlatform.SharedResources.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo.Util
{
    public static class DocumentUtility
    {
        public const string FileTransformTaskType = "FileTransformTask";
        public const string Shortcut = "Shortcut";

        public static IList<DocumentVersionDto> GetDocVersionInfo(IList<DocumentVersion> docVersions, IBIMResourceManager ResourceManager)
        {
            List<DocumentVersionDto> versionInfoes = new List<DocumentVersionDto>();
            foreach (DocumentVersion version in docVersions)
            {
                DocumentVersionDto versionInfo = GetDocumentVersionInfo(version, ResourceManager);
                if (versionInfo != null)
                {
                    versionInfoes.Add(versionInfo);
                }
            }
            return versionInfoes;
        }

        public static DocumentVersionDto GetDocumentVersionInfo(DocumentVersion version, IBIMResourceManager ResourceManager)
        {
            DocumentVersionDto versionInfo = null;
            if (version != null)
            {
                #region Todo
                //DocDomain.DocumentVersion docVersionObj = ObjectMapper.Map<DocumentVersion, DocDomain.DocumentVersion>(version);
                //docVersionObj.GetSizeDisplayAs(ResourceManager);
                //UserDataInfo creationUserInfo = ApplicationService.Instance.CacheService.GetUserInfo(version.CreationUserID);
                //docVersionObj.CreationUser = creationUserInfo.DisplayName;
                //docVersionObj.StatusStr = DocumentExtenstion.GetStatuDisplay(docVersionObj.Status); 
                //docVersionObj.DocNumber = string.Empty;// version.Document.DocNumber;
                //docVersionObj.Suffix = version.Suffix;
                //versionInfo = ObjectMapper.Map<DocDomain.DocumentVersion, DocumentVersionDto>(docVersionObj);
                //versionInfo.Tags = version.Tags;
                //versionInfo.RemotePath = GetDocumentFilePath(version);
                #endregion
            }
            return versionInfo;
        }

        private static string GetDocumentFilePath(DocumentVersion docVersionEntity)
        {
            return Path.Combine(AppSettings.Document.DocDataFolderPath, docVersionEntity.RemotePath);
        }
    }
}
