using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Application.Contracts.Entity;
using BIMPlatform.Application.Contracts.Events;
using BIMPlatform.Application.Contracts.UserDataInfo;
using BIMPlatform.Document;
using BIMPlatform.Repositories.Document;
using BIMPlatform.ToolKits.Helper;
using BIMPlatform.Users;
using BIMPlatform.Users.Repositories;
using Microsoft.AspNetCore.Http;
using Platform.ToolKits.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace BIMPlatform.DocumentService.impl
{
    public partial class DocumentService : BaseService, IDocumentService
    {
        private readonly IDataFilter DataFilter;
        private readonly IDocumentRepository DocumentRepository;
        private readonly IDocumentFolderRepository DocumentFolderRepository;
        private readonly IDocumentVersionRepository DocumentVersionRepository;
        private readonly IDocumentFolderCommonService DocumentFolderCommonService;
        private readonly IUserRepository UserRepository;

        private static object mobjLock = new object();
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

        public DocumentService(IDocumentRepository documentRepository,
            IDataFilter dataFilter,
            IDocumentFolderRepository documentFolderRepository,
            IDocumentVersionRepository documentVersionRepository,
            IDocumentFolderCommonService documentFolderCommonService,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            DataFilter = dataFilter;
            DocumentRepository = documentRepository;
            DocumentFolderRepository = documentFolderRepository;
            DocumentVersionRepository = documentVersionRepository;
            DocumentFolderCommonService = documentFolderCommonService;
            UserRepository = userRepository;
        }

        #region service
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadParams"></param>
        /// <returns></returns>
        public async Task<DocumentVersion> UploadFile(int projectID, DocumentFileDataInfo fileInfo)
        {
            DocumentVersion docVer = null;
            lock (mobjLock)
            {
                docVer = UploadFileInternal(fileInfo);
            }
            NotificationDocument(projectID, fileInfo, docVer);
            return docVer;
        }


        public Task<IList<DocumentVersion>> GetLatestDocVersionsByFolderInternal(long folderID, string suffix)
        {
            return GetLatestDocVersionsByFolder(folderID, suffix);
        }
        public async Task<IList<string>> DeleteDocumentsOfFolderInternal(int projectID, Guid userID, long folderID, bool requireRecycle, Guid recycleIdentity)
        {
            // Recycle a folder, auto recycle all documents of the folder but not record into Recycle Bin
            // Invoker will record folder recycled
            // Otherwise, delete the files of the folder automatically
            List<string> filePaths = new List<string>();

            if (requireRecycle)
            {
                IList<Document.Document> documents = DocumentRepository.FindList(doc => doc.FolderID == folderID && doc.Status == "Created");
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
                        DocumentVersionRepository.FindList(dv => dv.DocFolder.RecycleIdentity == recycleIdentity).GroupBy(dv => dv.Document.Id);
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

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="versionIDs"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DownloadFileItemDataInfo DownloadFiles(IList<long> versionIDs, Guid userId)
        {
            DownloadFileItemDataInfo fileItem = new DownloadFileItemDataInfo();
            string[] includePath = null; // new string[] { "Document" };

            if (versionIDs.Count() == 0)
            {
                throw new ArgumentException(L["DocumentFolderError:RequireDocumnetVersionIDParameter"]);
            }
            else if (versionIDs.Count() == 1)
            {
                long versionID = versionIDs[0];
                DocumentVersion docVersionEntity = DocumentVersionRepository.Query(ver => ver.Id == versionID, includePath).FirstOrDefault();
                string fileOriPath = GetDocumentFilePath(docVersionEntity);

                string viewFileRelativePath = string.Empty;
                string viewFileFullPath = string.Empty;

                GetDocFileViewInfo(docVersionEntity.Name, true, ref viewFileRelativePath, ref viewFileFullPath);

                File.Copy(fileOriPath, viewFileFullPath);

                fileItem.Name = docVersionEntity.Name;
                fileItem.Path = viewFileFullPath;
                fileItem.RelativePath = viewFileRelativePath;

                RecordOperation(docVersionEntity, userId, OperationRecordType.DownloadDocument);
            }
            else
            {
                includePath = new string[] { "DocFolder" };
                List<string[]> list = new List<string[]>();
                IList<DocumentVersion> docVersions = DocumentVersionRepository.Query(ver => versionIDs.Contains(ver.Id), includePath).ToList();
                string folderName = string.Empty;
                foreach (DocumentVersion version in docVersions)
                {
                    string path = GetDocumentFilePath(version);

                    string[] s = new string[2];
                    if (File.Exists(path))
                    {
                        s[0] = path;
                        s[1] = version.Name;
                    }
                    else
                    {
                        //ServerLogger.Error(string.Format("Cannot get file path for document {0}", version.Name));
                        continue;
                    }

                    if (string.IsNullOrEmpty(folderName))
                    {
                        folderName = version.DocFolder.Name;
                    }
                    list.Add(s);

                    RecordOperation(version, userId, OperationRecordType.DownloadDocument);
                }

                if (list.Count == 0)
                {
                    throw new Exception(L["DocumentFolderError:NoFileToDownload"]);
                }

                string fileName = folderName + ".zip";
                string error = "";

                string viewFileRelativePath = string.Empty;
                string viewFileFullPath = string.Empty;

                GetDocFileViewInfo(fileName, true, ref viewFileRelativePath, ref viewFileFullPath);

                if (ZipHelper.Pack(list, viewFileFullPath, 6, string.Empty, out error))
                {
                    fileItem.Name = fileName;
                    fileItem.Path = viewFileFullPath;
                    fileItem.RelativePath = viewFileRelativePath;
                }
            }

            if (File.Exists(fileItem.Path))
            {
                return fileItem;
            }
            else
            {
                throw new Exception(L["DocumentFolderError:NoFileToDownload"]);
            }
        }

        public void DeleteDocumentByVersionID(int projectID, Guid userID, long versionID, bool requireRecycle, Guid recycleIdentity)
        {
            IList<string> filePaths = DeleteDocumentByVersionIDInternal(projectID, userID, versionID, requireRecycle, recycleIdentity);
            FileUtil.DeleteFiles(filePaths);
        }
        /// <summary>
        /// Copy latest version of selected document to target folder
        /// </summary>
        /// <param name="targetFolderID"></param>
        /// <param name="documentIDs">document ID</param>
        /// <param name="userID"></param>
        public async Task CopyDocumentsToFolderAsync(int projectID, long targetFolderID, List<long> documentIDs, Guid userID)
        {
            DocumentFolder folder = DocumentFolderRepository.FindByKeyValues(targetFolderID);
            if (folder == null)
            {
                throw new ArgumentException(L["DocumentFolderError: FolderNotExist"]);
            }

            string error = string.Empty;
            if (!DocumentFolderCommonService.CanCopyDocuments(targetFolderID, documentIDs, out error))
            {
                throw new Exception(error);
            }

            IList<DocumentVersion> allLatestVersions = GetLatestDocVersionsByDocIDs(documentIDs);

            foreach (DocumentVersion version in allLatestVersions)
            {
                DocumentFileDataInfo fileInfo = new DocumentFileDataInfo()
                {
                    FolderID = targetFolderID,
                    Name = version.Name,
                    Suffix = version.Suffix,
                    CreationUserID = userID,
                    CustomizedProperties = XmlOption.LoadPropertys(version.Properties),
                    SupportDocNumber = true,
                    RequireTransform = true,
                    ProjectID = projectID,
                    Status = version.Status

                };
                string path = Path.Combine(CommonDefine.DocDataFolderPath, version.RemotePath);

                fileInfo.Stream = File.OpenRead(path);

                DocumentVersion docVer = UploadFileInternal(fileInfo);
            }
        }

        /// <summary>
        /// Move documents to target folder, including document versions
        /// </summary>
        /// <param name="targetFolderID"></param>
        /// <param name="documentIDs"></param>
        /// <param name="userID"></param>
        public async Task MoveDocumentsToFolderAsync(int projectID, long targetFolderID, List<long> documentIDs, Guid userID)
        {
            // TODO, check duplidate and override method

            DocumentFolder folder = DocumentFolderRepository.FindByKeyValues(targetFolderID);
            if (folder == null)
            {
                throw new ArgumentException(L["DocumentFolderError:FolderNotExist"]);
            }

            string error = string.Empty;
            if (!DocumentFolderCommonService.CanMoveDocuments(targetFolderID, documentIDs, out error))
            {
                throw new Exception(error);
            }

            IList<Document.Document> movedDocuments = DocumentRepository.FindList(doc => documentIDs.Contains(doc.Id));

            IList<string> documentNames = movedDocuments.Select(doc => doc.Name.ToLower()).ToList();

            // Check duplidate of target folder
            Dictionary<string, Document.Document> duplicateDocuments = DocumentRepository.FindList(doc => doc.FolderID == targetFolderID && documentNames.Contains(doc.Name.ToLower())).ToDictionary(doc => doc.Name.ToLower());

            List<string> removedPaths = new List<string>();
            foreach (Document.Document doc in movedDocuments)
            {
                // Upgrade version
                if (duplicateDocuments.ContainsKey(doc.Name.ToLower()))
                {
                    List<long> movedDocumentIDs = new List<long>();
                    movedDocumentIDs.Add(doc.Id);
                    string[] includePath = new string[] { "Document" };
                    IList<DocumentVersion> allLatestVersions = GetLatestDocVersionsByDocIDs(movedDocumentIDs, includePath);
                    DocumentVersion docVersion = allLatestVersions.FirstOrDefault();
                    if (docVersion != null)
                    {
                        DocumentFileDataInfo fileInfo = new DocumentFileDataInfo()
                        {
                            FolderID = targetFolderID,
                            Name = docVersion.Name,
                            Suffix = docVersion.Suffix,
                            CreationUserID = userID,
                            CustomizedProperties = XmlOption.LoadPropertys(docVersion.Properties),
                            SupportDocNumber = true,
                            RequireTransform = true,
                            ProjectID = projectID,
                            Status = docVersion.Status

                        };
                        string path = Path.Combine(CommonDefine.DocDataFolderPath, docVersion.RemotePath);

                        fileInfo.Stream = File.OpenRead(path);
                        DocumentVersion docVer = UploadFileInternal(fileInfo);

                        //TransformNewDocVersion(docVer);

                        IList<string> filePaths = DeleteDocumentByVersionIDInternal(projectID, userID, docVersion.Id, false, Guid.Empty);
                        removedPaths.AddRange(filePaths);
                    }
                }
                else
                {
                    doc.FolderID = targetFolderID;
                    foreach (var item in doc.DocumentVersions)
                    {
                        item.FolderID = targetFolderID;
                    }
                    DocumentRepository.UpdateAsync(doc);
                }
            }

            FileUtil.DeleteFiles(removedPaths);
        }

        #endregion

        #region private service
        private void NotificationDocument(int projectID, DocumentFileDataInfo fileInfo, DocumentVersion docVer)
        {
            if (projectID > 0)
            {
                if (fileInfo.NotificationUserId != null)
                {
                    // For each @User, send reminder to him
                    foreach (var userId in fileInfo.NotificationUserId)
                    {
                        SubscribeDocumentVersionEvent(docVer, userId, "DocumentUploadReminder", NotificationType.EmailAndNotification);
                    }

                    // Subscribe receipt event for this document version, so that whenever user viewed the document notification
                    // send receipt back to the creation user
                    SubscribeDocumentVersionEvent(docVer, docVer.CreationUserID, "DocumentReceipt", NotificationType.Notification);
                }

                #region Todo
                //ProjectRelevantEntityDataInfo entityInfo = new ProjectRelevantEntityDataInfo()
                //{
                //    ProjectID = projectID,
                //    EntityClassName = "DocumentVersion",
                //    EntityKey = "ID",
                //    EntityValue = docVer.ID.ToString(),
                //    RelevantObject = docVer
                //};
                //RaiseEvent(entityInfo, "DocumentUploadReminder");
                #endregion

                RecordOperation(docVer, docVer.CreatorId.GetValueOrDefault(), OperationRecordType.DocumentUpload);

            }
        }

        private async Task<IList<DocumentVersion>> GetLatestDocVersionsByFolder(long folderID, string suffix, params string[] includePath)
        {
            // TO improve performance, get folder id and name from DocumentVersion directly to avoid query several tables
            IList<DocumentVersion> orderedLatestVersions = null;
            if (string.IsNullOrEmpty(suffix))
            {
                orderedLatestVersions = DocumentVersionRepository.FindList(ver => ver.FolderID == folderID && mcolVisibleStatuses.Contains(ver.Status))
                    .GroupBy(ver => ver.Document.Id).Select(a => a.OrderByDescending(v => v.Version).FirstOrDefault()).OrderBy(ver => ver.Name).ToList();
            }
            else
            {
                orderedLatestVersions = DocumentVersionRepository.FindList(ver => ver.FolderID == folderID && ver.Suffix == suffix && mcolVisibleStatuses.Contains(ver.Status))
                    .GroupBy(ver => ver.Document.Id).Select(a => a.OrderByDescending(v => v.Version).FirstOrDefault()).OrderBy(ver => ver.Name).ToList();
            }

            return orderedLatestVersions;
        }


        private IList<string> DeleteDocument(int projectID, Guid userID, Document.Document document, bool requireRecycle, Guid recycleIdentity, bool canRestoreAsSource)
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

        private void RecordOperation(DocumentVersion documentVersion, Guid userID, OperationRecordType type)
        {
            if (userID == Guid.Empty)
            {
                throw new ArgumentException(L["UserError:NullUserId"]);
            }
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
        private IList<string> DeleteTransformedDocument(int projectID, Guid userID, DocumentVersion version)
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
        private DocumentVersion UploadFileInternal(DocumentFileDataInfo fileInfo)
        {
            if (string.IsNullOrEmpty(fileInfo.Name) || string.IsNullOrEmpty(fileInfo.Suffix))
            {
                throw new ArgumentException(L["DocumentError:NullNameOrSuffix"]);
            }

            DocumentFolder folderEntity = DocumentFolderRepository.FirstOrDefault(n => n.Id == fileInfo.FolderID);
            if (folderEntity == null)
            {
                throw new ArgumentException(L["DocumentFolderError:FolderNotExist"]);
            }

            if (fileInfo.CreationUserID == Guid.Empty)
            {
                throw new ArgumentException(L["DocumentFolderError:RequireCurrentUserInfo"]);
            }

            UserDto userInfo = new UserDto();
            #region Todo
            AppUser user = UserRepository.FindByKeyValues(CurrentUser.Id.Value);
            userInfo = ObjectMapper.Map<AppUser, UserDto>(user);
            //UserDataInfo userInfo = ApplicationService.Instance.CacheService.GetUserInfo(fileInfo.CreationUserID);
            #endregion

            DocumentFolder targetFolder = GetUploadFolder(folderEntity, fileInfo);

            Document.Document documentEntity =
                DocumentRepository.FirstOrDefault(doc => doc.Name == fileInfo.Name && doc.FolderID == targetFolder.Id && doc.Suffix == fileInfo.Suffix && mcolVisibleStatuses.Contains(doc.Status));
            Application.Contracts.DocumentDataInfo.Domain.Document documentObj = new Application.Contracts.DocumentDataInfo.Domain.Document();
            documentObj.Name = fileInfo.Name;
            documentObj.Suffix = fileInfo.Suffix.ToLower();
            documentObj.FolderID = targetFolder.Id;
            documentObj.Status = fileInfo.Status == null ? "Created" : fileInfo.Status;

            if (SupportDocumentNumber && fileInfo.SupportDocNumber)
                documentObj.DocNumber = GetDocNumber(fileInfo, documentObj);

            if (documentEntity != null)
            {
                documentObj = ObjectMapper.Map<Document.Document, Application.Contracts.DocumentDataInfo.Domain.Document>(documentEntity);
                foreach (DocumentVersion docVersionEntity in documentEntity.DocumentVersions.ToList())
                {
                    Application.Contracts.DocumentDataInfo.Domain.DocumentVersion docVersionObj = ObjectMapper.Map<DocumentVersion, Application.Contracts.DocumentDataInfo.Domain.DocumentVersion>(docVersionEntity);
                    documentObj.Versions.Add(docVersionObj);
                }
            }
            else
            {
                documentEntity = ObjectMapper.Map<Application.Contracts.DocumentDataInfo.Domain.Document, Document.Document>(documentObj);
                DocumentRepository.Add(documentEntity);
            }

            Application.Contracts.DocumentDataInfo.Domain.DocumentVersion nextDocVersionObj = documentObj.CreateNextVersion(targetFolder.Id, fileInfo, userInfo);
            DocumentVersion nextDocVerionEntity = ObjectMapper.Map<Application.Contracts.DocumentDataInfo.Domain.DocumentVersion, DocumentVersion>(nextDocVersionObj);
            nextDocVerionEntity.Document = documentEntity;
            if (fileInfo.Status == "OnVerified")
            {
                #region Todo
                //IList<Dictionary<string, object>> verifiedUsers = ProjectUserRoleService.GetProjectUnitAndUser(fileInfo.ProjectID, fileInfo.VerifiedPermissionName);
                //List<int> verifiedUserIDs = new List<int>();
                //foreach (var item in verifiedUsers)
                //{
                //    verifiedUserIDs.Add(int.Parse(item["UserID"].ToString()));
                //}
                //SendNotification("DocumentVersionVerifiedNotification", nextDocVerionEntity, verifiedUserIDs, fileInfo.ProjectID);
                #endregion
            }

            string parentFolder = Path.Combine(CommonDefine.DocDataFolderPath, nextDocVersionObj.ParentFolderName);
            if (!Directory.Exists(parentFolder))
            {
                Directory.CreateDirectory(parentFolder);
            }

            string remoteFullPath = Path.Combine(CommonDefine.DocDataFolderPath, nextDocVersionObj.RemotePath);
            try
            {
                using (FileStream writer = new FileStream(remoteFullPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[512];
                    int c = 0;
                    while ((c = fileInfo.Stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(buffer, 0, c);
                    }
                }
            }
            finally
            {
                fileInfo.Stream.Close();
            }

            string[] tags = fileInfo.Tags != null ? fileInfo.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };
            int[] userIDs = fileInfo.NotificationUserId != null ? fileInfo.NotificationUserId.ToArray() : new int[] { };
            //Todo
            //UserPreferenceService.SaveUserTagsAndRecentUsers(fileInfo.CreationUserID, fileInfo.ProjectID, tags, userIDs);
            nextDocVerionEntity.Tags = fileInfo.Tags;

            DocumentVersionRepository.Add(nextDocVerionEntity);
            if (fileInfo.RequireTransform && fileInfo.ProjectID > 0)
            {
                TransformNewDocVersion(fileInfo.ProjectID, nextDocVerionEntity);
            }
            return nextDocVerionEntity;
        }

        private string GetDocNumber(DocumentFileDataInfo fileInfo, Application.Contracts.DocumentDataInfo.Domain.Document documentObj)
        {
            string folderName = DocumentFolderRepository.FirstOrDefault(n => n.Id == fileInfo.FolderID).Name;
            DocumentFolder folder = GetParentFolder(fileInfo.FolderID);
            string projectName = folder.ProjectRootFolders.FirstOrDefault().Project.Name;
            string docNumberingRule = ConfigurationManager.AppSettings["DocNumberingRule"];
            documentObj.SetDefaultProvideKeyVariableValue(projectName, folderName, null);
            string defaultDocRule = documentObj.GetAutoId(docNumberingRule);
            string replaceWord = defaultDocRule.Replace("{Sequence}", "%");

            var currentNumber = DocumentRepository.FindList(t => t.DocNumber.Contains(replaceWord)).Select(d => d.DocNumber).ToList();
            int sequenceIndex = docNumberingRule.Split('-').ToList().IndexOf("{Sequence}");
            int docNumber = 0;
            if (sequenceIndex > -1)
            {
                try { docNumber = currentNumber.Max(a => int.Parse(a.Split('-')[sequenceIndex])); }
                catch { }
            }
            documentObj.SetDefaultProvideKeyVariableValue(projectName, folderName, docNumber);
            return documentObj.GetAutoId(docNumberingRule);
        }

        private DocumentFolder GetParentFolder(long folderID)
        {
            DocumentFolder folder = DocumentFolderRepository.FindByKeyValues(folderID);
            if (folder.ParentFolderID.HasValue)
            {
                return GetParentFolder(folder.ParentFolderID.Value);
            }
            else
            {
                return folder;
            }
        }

        private DocumentFolder GetUploadFolder(DocumentFolder selectedFolder, DocumentFileDataInfo fileInfo)
        {
            DocumentFolder targetFolder = selectedFolder;
            if (!string.IsNullOrEmpty(fileInfo.ClientRelativePath) && fileInfo.ClientRelativePath.LastIndexOf("/") > 0)
            {
                int index = fileInfo.ClientRelativePath.LastIndexOf("/");
                string folderPath = fileInfo.ClientRelativePath.Substring(0, index);
                targetFolder = DocumentFolderCommonService.GetOrCreateFolderByPathInternal(selectedFolder, fileInfo.CreationUserID, folderPath);
            }

            return targetFolder;
        }

        private void TransformNewDocVersion(int projectID, DocumentVersion docVer)
        {
            if (docVer != null && SupportPreviewDocument)
            {
                var docExtention = docVer.Suffix.ToLower();

                #region Todo
                //// Require transform
                //DocumentPreviewConfigItem item = DocumentPreviewConfigService.GetPreviewConfigBySuffix(docExtention);
                //if (item != null && !string.IsNullOrEmpty(item.TransformHandler))
                //{
                //    ScheduleTransformFileTask(docVer, projectID, item);
                //}
                #endregion
            }
        }

        private void SubscribeDocumentVersionEvent(DocumentVersion docVer, int userID, string eventSystemName, NotificationType type)
        {
            EntityDataInfo entityInfo = new EntityDataInfo() { EntityClassName = "DocumentVersion", EntityKey = "ID", EntityValue = docVer.Id.ToString() };
            SubscribeEvent(entityInfo, userID, eventSystemName, type);
        }

        private void GetDocFileViewInfo(string fileName, bool encodeFileName, ref string viewFileRelativePath, ref string viewFileFullPath)
        {
            string docViewRelativeFolder = CommonDefine.DocViewNewSubFolderRelativePath;
            string viewFileFolderPath = string.Empty;
            GetDocFileViewInfo(docViewRelativeFolder, fileName, encodeFileName, ref viewFileRelativePath, ref viewFileFolderPath, ref viewFileFullPath);
        }

        private void GetDocFileViewInfo(string docViewRelativeFolder, string fileName, bool encodeFileName, ref string viewFileRelativePath, ref string viewFileFolderPath, ref string viewFileFullPath)
        {
            viewFileFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, docViewRelativeFolder);
            if (!Directory.Exists(viewFileFolderPath))
            {
                Directory.CreateDirectory(viewFileFolderPath);
            }

            string name = encodeFileName ? HttpUtility.UrlEncode(fileName) : fileName;
            // Fix bug that return / for web usage directly
            // viewFileRelativePath = Path.Combine(docViewRelativeFolder, name);
            string relativeFolderPath = docViewRelativeFolder.Replace("\\", "/");
            if (!relativeFolderPath.EndsWith("/"))
            {
                relativeFolderPath = relativeFolderPath + "/";
            }
            viewFileRelativePath = relativeFolderPath + name;
            viewFileFullPath = Path.Combine(viewFileFolderPath, fileName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        /// <param name="versionID"></param>
        /// <param name="requireRecycle"></param>
        /// <param name="recycleIdentity"></param>

        public IList<string> DeleteDocumentByVersionIDInternal(int projectID, Guid userID, long versionID, bool requireRecycle, Guid recycleIdentity)
        {
            List<string> filePaths = new List<string>();

            DocumentVersion dv = DocumentVersionRepository.FirstOrDefault(n => n.Id == versionID);
            if (dv == null)
            {
                throw new ArgumentException(L["DocumentError:NotExist"]);
            }

            return DeleteDocumentByVersion(projectID, userID, dv, requireRecycle, recycleIdentity);
        }

        private IList<string> DeleteDocumentByVersion(int projectID, Guid userID, DocumentVersion docVersion, bool requireRecycle, Guid recycleIdentity)
        {
            Document.Document document = DocumentRepository.FirstOrDefault(doc => doc.Id == docVersion.Document.Id, new string[] { "DocumentVersions" });
            return DeleteDocument(projectID, userID, document, requireRecycle, recycleIdentity, true);
        }


        private IList<DocumentVersion> GetLatestDocVersionsByDocIDs(IList<long> documentIDs, params string[] includePath)
        {
            // Group by document and then ordered by version for each document
            IQueryable<DocumentVersion> orderedLatestVersions =
                  DocumentVersionRepository.Query(ver => documentIDs.Contains(ver.Document.Id) && mcolVisibleStatuses.Contains(ver.Status), includePath).GroupBy(ver => ver.Document.Id)
                  .Select(a => a.OrderByDescending(v => v.Version).FirstOrDefault()).OrderBy(v => v.Name);

            return orderedLatestVersions.ToList();
        }


        #endregion
        public override void SubscribeEvent(EntityDataInfo entityInfo, int userID, string eventSystemName, NotificationType type)
        {
            base.SubscribeEvent(entityInfo, userID, eventSystemName, type);
        }

    }
}
