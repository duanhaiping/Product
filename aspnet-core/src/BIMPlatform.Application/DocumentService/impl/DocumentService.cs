using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Repositories.Document;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Platform.ToolKits.Extensions;
using BIMPlatform.Document;
using System.IO;
using BIMPlatform.Application.Contracts.Entity;
using BIMPlatform.Application.Contracts.Util;
using Platform.ToolKits.Common;

namespace BIMPlatform.DocumentService.impl
{
    public partial class DocumentService : BaseService, IDocumentService
    {
        private readonly IDataFilter DataFilter;
        private readonly IDocumentRepository DocumentRepository;
        private readonly IDocumentFolderRepository DocumentFolderRepository;
        private readonly IDocumentVersionRepository DocumentVersionRepository;

        private string[] mcolVisibleStatuses = new string[] { "Created", "Released", "OnVerified" };
        public virtual bool SupportTransformDocument
        {
            get
            {
                return true;
            }
        }

        public virtual bool SupportPreviewDocument
        {
            get
            {
                return true;
            }
        }

        public virtual bool SupportDocumentNumber
        {
            get { return false; }
        }

        public DocumentService(IDocumentRepository documentRepository, IDataFilter dataFilter, IDocumentFolderRepository documentFolderRepository, IDocumentVersionRepository documentVersionRepository)
        {
            DataFilter = dataFilter;
            DocumentRepository = documentRepository;
            DocumentFolderRepository = documentFolderRepository;
            DocumentVersionRepository = documentVersionRepository;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadParams"></param>
        /// <returns></returns>
        public async Task UploadFile(DocumentDto document)
        {
            //DocumentVersion docVer = null;
            //docVer = UploadFileInternal(document);
        }

        public Task<IList<DocumentVersion>> GetLatestDocVersionsByFolderInternal(long folderID, string suffix)
        {
            return GetLatestDocVersionsByFolder(folderID, suffix);
        }

        private async Task<IList<DocumentVersion>> GetLatestDocVersionsByFolder(long folderID, string suffix, params string[] includePath)
        {
            // TO improve performance, get folder id and name from DocumentVersion directly to avoid query several tables
            IList<DocumentVersion> orderedLatestVersions = null;
            if (string.IsNullOrEmpty(suffix))
            {
                orderedLatestVersions = (await DocumentVersionRepository.GetListAsync()).Where(ver => ver.FolderID == folderID && mcolVisibleStatuses.Contains(ver.Status))
                    .GroupBy(ver => ver.Document.Id).Select(a => a.OrderByDescending(v => v.Version).FirstOrDefault()).OrderBy(ver => ver.Name).ToList();
            }
            else
            {
                orderedLatestVersions = (await DocumentVersionRepository.GetListAsync()).Where(ver => ver.FolderID == folderID && ver.Suffix == suffix && mcolVisibleStatuses.Contains(ver.Status))
                    .GroupBy(ver => ver.Document.Id).Select(a => a.OrderByDescending(v => v.Version).FirstOrDefault()).OrderBy(ver => ver.Name).ToList();
            }

            return orderedLatestVersions;
        }

        public async Task<IList<string>> DeleteDocumentsOfFolderInternal(int projectID, int userID, long folderID, bool requireRecycle, Guid recycleIdentity)
        {
            // Recycle a folder, auto recycle all documents of the folder but not record into Recycle Bin
            // Invoker will record folder recycled
            // Otherwise, delete the files of the folder automatically
            List<string> filePaths = new List<string>();

            if (requireRecycle)
            {
                IList<Document.Document> documents = (await DocumentRepository.GetListAsync()).Where(doc => doc.FolderID == folderID && doc.Status == "Created").ToList();
                foreach (Document.Document doc in documents)
                {
                    DeleteDocument(projectID, userID, doc, true, recycleIdentity, false);
                }
            }
            else
            {
                IList<DocumentVersion> docVers = null;
                if (recycleIdentity != Guid.Empty)
                {
                    IEnumerable<IGrouping<long, DocumentVersion>> groupedDocs =
                        (await DocumentVersionRepository.GetListAsync()).Where(dv => dv.DocFolder.RecycleIdentity == recycleIdentity).GroupBy(dv => dv.Document.Id).ToList();
                    docVers = new List<DocumentVersion>();

                    foreach (IGrouping<long, DocumentVersion> groupDoc in groupedDocs)
                    {
                        docVers.Add(groupDoc.First());
                    }
                }
                else
                {
                    docVers = GetLatestDocVersionsByFolder(folderID, null).Result;
                }
                if (docVers != null)
                {
                    IList<Document.Document> docs = docVers.Select(dv => dv.Document).Distinct().ToList();
                    foreach (Document.Document doc in docs)
                    {
                        IList<string> paths = DeleteDocument(projectID, userID, doc, false, recycleIdentity, false);
                        filePaths.AddRange(paths);
                    }
                }
            }
            return filePaths;
        }

        private IList<string> DeleteDocument(int projectID, int userID, Document.Document document, bool requireRecycle, Guid recycleIdentity, bool canRestoreAsSource)
        {
            List<string> filePaths = new List<string>();
            OperationRecordType opType = OperationRecordType.RecycleDocument;
            if (requireRecycle)
            {
                document.RecycleIdentity = recycleIdentity;
                document.Status = "Recycled";
                DocumentRepository.UpdateAsync(document);

                DocumentVersion latestDocVer = null;

                // Set all versions as recycled
                foreach (DocumentVersion docVer in document.DocumentVersions)
                {
                    if (latestDocVer == null || latestDocVer.Version < docVer.Version)
                    {
                        latestDocVer = docVer;
                    }

                    docVer.Status = "Recycled";
                    DocumentVersionRepository.UpdateAsync(docVer);
                    EntityDataInfo fromEntity = new EntityDataInfo()
                    {
                        EntityClassName = "DocumentVersion",
                        EntityKey = "ID",
                        EntityValue = docVer.Id.ToString()
                    };

                    #region Todo
                    //IList<object> tasks = EntityLinkService.GetSubLinkEntities(fromEntity, DocumentUtility.FileTransformTaskType);
                    //if (tasks != null)
                    //{
                    //    foreach (object taskObj in tasks)
                    //    {
                    //        ScheduledTask task = taskObj as ScheduledTask;
                    //        ScheduledTaskDataInfo taskInfo = new ScheduledTaskDataInfo() { ID = task.ID };
                    //        ScheduledTaskService.DeleteScheduledTaskInternal(taskInfo);
                    //    }
                    //}
                    #endregion
                }

                #region Todo
                //// if delete document directly, it's true
                //// if delete document folder directly, it's false
                //if (canRestoreAsSource)
                //{
                //    // Only put the select version into recycle bin
                //    List<UserRecycledEntityDataInfo> entities = new List<UserRecycledEntityDataInfo>();
                //    entities.Add(new UserRecycledEntityDataInfo()
                //    {
                //        EntityName = latestDocVer.Name,
                //        EntityClassName = "DocumentVersion",
                //        EntityKey = "ID",
                //        EntityValue = latestDocVer.ID.ToString(),
                //        Status = "Recycled",
                //        ProjectID = projectID,
                //        UserID = userID,
                //        CreationDate = DateTime.Now
                //    });
                //    RecycledEntityService.RecycleEntities(entities);
                //}

                //RecordOperation(latestDocVer, userID, opType);
                #endregion
            }
            else
            {
                opType = OperationRecordType.CleanDocument;
                DocumentVersion latestDocVer = null;
                // Delete from system
                foreach (DocumentVersion docVer in document.DocumentVersions)
                {
                    if (latestDocVer == null || latestDocVer.Version < docVer.Version)
                    {
                        latestDocVer = docVer;
                    }

                    string path = GetDocumentFilePath(docVer);
                    filePaths.Add(path);

                    IList<string> transformedPaths = DeleteTransformedDocument(projectID, userID, docVer);
                    if (transformedPaths != null)
                    {
                        filePaths.AddRange(transformedPaths);
                    }
                }

                #region 旧框架Context相关 暂时放置
                //IEFRepositoryContext context = this.Context as IEFRepositoryContext;
                //context.Context.Entry(document).Collection(n => n.DocumentVersions).Load();
                //var mfdv = context.Context.Entry(document).Collection(n => n.ModelFolderDocumentVersions);
                //if (mfdv != null)
                //{
                //    mfdv.Load();
                //}
                //var em = context.Context.Entry(document).Collection(n => n.EntityModels);
                //if (em != null)
                //{
                //    em.Load();
                //}
                #endregion

                // Fix bug that cannot get document after deleting
                RecordOperation(latestDocVer, userID, opType);

                /*
                 * 此处需要删除 因为框架暂时没有封装批量删除
                 * 导致此处暂时无法操作 封装好后应处理此行代码
                 */
                //DocumentVersionRepository.Delete(document.DocumentVersions.ToList());

                DocumentRepository.DeleteAsync(document);
            }

            return filePaths;
        }

        protected void RecordOperation(DocumentVersion documentVersion, int userID, OperationRecordType type)
        {
            #region Todo
            //if (type == OperationRecordType.DocumentUpload)
            //{
            //    RelevantEntityDataInfo entityInfo = new RelevantEntityDataInfo() { EntityClassName = "DocumentVersion", EntityKey = "ID", EntityValue = documentVersion.ID.ToString(), RelevantObject = documentVersion };
            //    RecordOperation(entityInfo, userID, type.ToString());
            //}
            //else if (type == OperationRecordType.DownloadDocument)
            //{
            //    RelevantEntityDataInfo entityInfo = new RelevantEntityDataInfo() { EntityClassName = "Document", EntityKey = "ID", EntityValue = documentVersion.DocumentID.ToString(), RelevantObject = documentVersion };
            //    RecordOperation(entityInfo, userID, type.ToString());
            //}
            //else if (type == OperationRecordType.RecycleDocument || type == OperationRecordType.CleanDocument)
            //{
            //    OnDocumentDeleted(documentVersion, userID);
            //}
            //else if (type == OperationRecordType.DeleteDocumentVersion)
            //{
            //    OnDocumentVersionDeleted(documentVersion, userID);
            //}
            #endregion
        }


        /// <summary>
        /// Delete transformed document permanently
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private IList<string> DeleteTransformedDocument(int projectID, int userID, DocumentVersion version)
        {
            List<string> filePaths = new List<string>();

            #region Todo
            //if (SupportTransformDocument && ScheduledTaskService != null && TaskHandlerRepository != null)
            //{
            //    // Get ScheduledTask
            //    EntityDataInfo fromEntity = new EntityDataInfo() { EntityClassName = "DocumentVersion", EntityKey = "ID", EntityValue = version.ID.ToString() };
            //    IList<object> tasks = EntityLinkService.GetSubLinkEntities(fromEntity, DocumentUtility.FileTransformTaskType);
            //    if (tasks != null)
            //    {
            //        foreach (object taskObj in tasks)
            //        {
            //            ScheduledTask task = taskObj as ScheduledTask;
            //            ScheduledTaskDataInfo taskInfo = new ScheduledTaskDataInfo() { ID = task.ID };
            //            ScheduledTaskService.DeleteScheduledTaskInternal(taskInfo);
            //        }
            //    }

            //    // Get Transformed file
            //    IList<DocumentAssociationDataInfo> transformedDocs = DocAssociationService.GetAllAssociatedDocumentsByEntity(fromEntity, "DocTransform");
            //    if (transformedDocs != null)
            //    {
            //        foreach (DocumentAssociationDataInfo transformedDoc in transformedDocs)
            //        {
            //            IList<string> paths = DeleteDocumentByVersionIDInternal(projectID, userID, transformedDoc.DocVersionID, false, Guid.Empty);
            //            if (paths != null)
            //            {
            //                filePaths.AddRange(paths);
            //            }
            //        }
            //    }
            //}
            #endregion

            return filePaths;
        }

        private string GetDocumentFilePath(DocumentVersion docVersionEntity)
        {
            return Path.Combine(CommonDefine.DocDataFolderPath, docVersionEntity.RemotePath);
        }

        /// <summary>
        /// Upload file and create version entity, without saving into DB
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public DocumentVersion UploadFileInternal(DocumentDto fileInfo)
        {
            return new DocumentVersion();

            #region Todo
            //if (string.IsNullOrEmpty(fileInfo.Name) || string.IsNullOrEmpty(fileInfo.Suffix))
            //{
            //    throw new ArgumentException(L["DocumentError:NullNameOrSuffix"]);
            //}

            //DocumentFolder folderEntity = DocumentFolderRepository.FirstOrDefault(n => n.Id == fileInfo.FolderID);
            //if (folderEntity == null)
            //{
            //    throw new ArgumentException(L["DocumentFolderError:FolderNotExist"]);
            //}

            //if (fileInfo.CreationUserID <= 0)
            //{
            //    throw new ArgumentException(L["DocumentFolderError:RequireCurrentUserInfo"]);
            //}

            //#region Todo
            ////UserDataInfo userInfo = ApplicationService.Instance.CacheService.GetUserInfo(fileInfo.CreationUserID);
            //#endregion

            //DocumentFolder targetFolder = GetUploadFolder(folderEntity, fileInfo);

            //Document.Document documentEntity =
            //    DocumentRepository.FirstOrDefault(doc => doc.Name == fileInfo.Name && doc.FolderID == targetFolder.Id && doc.Suffix == fileInfo.Suffix && mcolVisibleStatuses.Contains(doc.Status));
            //Application.Contracts.DocumentDataInfo.Domain.Document documentObj = new Application.Contracts.DocumentDataInfo.Domain.Document();
            //documentObj.Name = fileInfo.Name;
            //documentObj.Suffix = fileInfo.Suffix.ToLower();
            //documentObj.FolderID = targetFolder.Id;
            //documentObj.Status = fileInfo.Status == null ? "Created" : fileInfo.Status;

            //if (SupportDocumentNumber && fileInfo.SupportDocNumber)
            //    documentObj.DocNumber = GetDocNumber(fileInfo, documentObj);

            //if (documentEntity != null)
            //{
            //    documentObj = SimpleObjectMapper.CreateTargetObject<EFModel.Document, Domain.Document>(documentEntity);
            //    foreach (DocumentVersion docVersionEntity in documentEntity.DocumentVersions.ToList())
            //    {
            //        Domain.DocumentVersion docVersionObj = SimpleObjectMapper.CreateTargetObject<DocumentVersion, Domain.DocumentVersion>(docVersionEntity);
            //        documentObj.Versions.Add(docVersionObj);
            //    }
            //}
            //else
            //{
            //    documentEntity = SimpleObjectMapper.CreateTargetObject<Domain.Document, EFModel.Document>(documentObj);
            //    DocumentRepository.Add(documentEntity);
            //}

            //Domain.DocumentVersion nextDocVersionObj = documentObj.CreateNextVersion(targetFolder.ID, fileInfo, userInfo);
            //DocumentVersion nextDocVerionEntity = SimpleObjectMapper.CreateTargetObject<Domain.DocumentVersion, DocumentVersion>(nextDocVersionObj);
            //nextDocVerionEntity.Document = documentEntity;
            //if (fileInfo.Status == "OnVerified")
            //{
            //    IList<Dictionary<string, object>> verifiedUsers = ProjectUserRoleService.GetProjectUnitAndUser(fileInfo.ProjectID, fileInfo.VerifiedPermissionName);
            //    List<int> verifiedUserIDs = new List<int>();
            //    foreach (var item in verifiedUsers)
            //    {
            //        verifiedUserIDs.Add(int.Parse(item["UserID"].ToString()));
            //    }
            //    SendNotification("DocumentVersionVerifiedNotification", nextDocVerionEntity, verifiedUserIDs, fileInfo.ProjectID);
            //}

            //string parentFolder = Path.Combine(CommonDefine.DocDataFolderPath, nextDocVersionObj.ParentFolderName);
            //if (!Directory.Exists(parentFolder))
            //{
            //    Directory.CreateDirectory(parentFolder);
            //}

            //string remoteFullPath = Path.Combine(CommonDefine.DocDataFolderPath, nextDocVersionObj.RemotePath);
            //try
            //{
            //    using (FileStream writer = new FileStream(remoteFullPath, FileMode.Create, FileAccess.Write))
            //    {
            //        byte[] buffer = new byte[512];
            //        int c = 0;
            //        while ((c = fileInfo.Stream.Read(buffer, 0, buffer.Length)) > 0)
            //        {
            //            writer.Write(buffer, 0, c);
            //        }
            //    }
            //}
            //finally
            //{
            //    fileInfo.Stream.Close();
            //}

            //string[] tags = fileInfo.Tags != null ? fileInfo.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };
            //int[] userIDs = fileInfo.NotificationUserId != null ? fileInfo.NotificationUserId.ToArray() : new int[] { };
            //UserPreferenceService.SaveUserTagsAndRecentUsers(fileInfo.CreationUserID, fileInfo.ProjectID, tags, userIDs);
            //nextDocVerionEntity.Tags = fileInfo.Tags;

            //DocumentVersionRepository.Add(nextDocVerionEntity);
            //if (fileInfo.RequireTransform && fileInfo.ProjectID > 0)
            //{
            //    TransformNewDocVersion(fileInfo.ProjectID, nextDocVerionEntity);
            //}
            //return nextDocVerionEntity;
            #endregion
        }

        public  string GetDocNumber(DocumentFileDataInfo fileInfo, Application.Contracts.DocumentDataInfo.Domain.Document documentObj)
        {
            string folderName = string.Empty;
            return folderName;

            #region Todo
            //string folderName = FolderRepository.FirstOrDefault(n => n.ID == fileInfo.FolderID).Name;
            ////string folderName = FolderService.GetFolder(fileInfo.FolderID).Name;
            //DocumentFolder folder = GetParentFolder(fileInfo.FolderID);
            //string projectName = folder.ProjectRootFolders.FirstOrDefault().Project.Name;
            //string docNumberingRule = ConfigurationManager.AppSettings["DocNumberingRule"];
            //documentObj.SetDefaultProvideKeyVariableValue(projectName, folderName, null);
            //string defaultDocRule = documentObj.GetAutoId(docNumberingRule);
            //string replaceWord = defaultDocRule.Replace("{Sequence}", "%");

            //string sql = $"select * from Document where DocNumber like '{replaceWord}'";
            //var currentNumber = DocumentRepository.SqlQueryList(sql).Select(d => d.DocNumber).ToList();
            //int sequenceIndex = docNumberingRule.Split('-').ToList().IndexOf("{Sequence}");
            //int docNumber = 0;
            //if (sequenceIndex > -1)
            //{
            //    try { docNumber = currentNumber.Max(a => int.Parse(a.Split('-')[sequenceIndex])); }
            //    catch { }
            //}
            //documentObj.SetDefaultProvideKeyVariableValue(projectName, folderName, docNumber);
            //return documentObj.GetAutoId(docNumberingRule);
            #endregion
        }

        private DocumentFolder GetUploadFolder(DocumentFolder selectedFolder, DocumentDto fileInfo)
        {
            DocumentFolder targetFolder = selectedFolder;
            if (!string.IsNullOrEmpty(fileInfo.ClientRelativePath) && fileInfo.ClientRelativePath.LastIndexOf("/") > 0)
            {
                int index = fileInfo.ClientRelativePath.LastIndexOf("/");
                string folderPath = fileInfo.ClientRelativePath.Substring(0, index);

                #region Todo
                //targetFolder = FolderDocumentCommonService.GetOrCreateFolderByPathInternal(selectedFolder, fileInfo.CreationUserID, folderPath);
                #endregion
            }

            return targetFolder;
        }
    }
}
