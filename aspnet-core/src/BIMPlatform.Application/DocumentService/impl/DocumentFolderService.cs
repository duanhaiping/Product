using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Document;
using BIMPlatform.DocumentService;
using BIMPlatform.Repositories.Document;
using BIMPlatform.ToolKits.Helper;
using log4net.Repository.Hierarchy;
using Platform.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace BIMPlatform.DocumentService.impl
{
    public partial class DocumentFolderService : BaseService, IDocumentFolderService
    {
        private readonly IDataFilter DataFilter;
        private readonly IDocumentFolderRepository DocumentFolderRepository;

        private readonly IDocumentService DocumentService;
        private readonly IDocumentFolderCommonService DocumentFolderCommonService;

        private List<string> VerifieFolderList { get; set; }

        public DocumentFolderService(IDataFilter dataFilter, IDocumentFolderRepository documentFolderRepository,
            IDocumentService documentService, IDocumentFolderCommonService documentFolderCommonService)
        {
            DataFilter = dataFilter;
            DocumentFolderRepository = documentFolderRepository;
            DocumentService = documentService;
            DocumentFolderCommonService = documentFolderCommonService;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="parentFolderID"></param>
        /// <param name="folderName"></param>
        /// <param name="creationUserID"></param>
        /// <returns></returns>
        public long CreateFolder(long? parentFolderID, string folderName)
        {
            return DocumentFolderCommonService.CreateFolder(parentFolderID, folderName);
        }

        /// <summary>
        /// 修改文件夹名称
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public bool RenameFolderName(long folderId, string newName)
        {
            DocumentFolder folder = DocumentFolderRepository.FirstOrDefault(n => n.Id == folderId);
            if (folder == null)
            {
                throw new ArgumentException(L["DocumentFolderError:FolderNotExist"]);
            }

            // No parent, can be renamed as any folder name
            if (folder.ParentFolderID.HasValue)
            {
                // Check the folder's parent folder has the same folder
                DocumentFolder existingFolder = DocumentFolderRepository.FirstOrDefault(f => f.Id != folderId && f.ParentFolderID == folder.ParentFolderID && f.Name == newName && f.Status == "Created");
                if (existingFolder != null)
                {
                    throw new ArgumentException(L["DocumentFolderError:DuplicateFolderName"]);
                }
            }

            folder.Name = newName;
            DocumentFolderRepository.UpdateAsync(folder);
            return true;
        }

        /// <summary>
        /// 删除文件夹及其子文件夹
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        /// <param name="folderID"></param>
        /// <param name="requireRecycle"></param>
        /// <param name="recycleIdentity"></param>
        /// <returns></returns>
        public bool DeleteFolder(int projectID, int userID, long folderID, bool requireRecycle, Guid recycleIdentity)
        {
            DocumentFolder folder = DocumentFolderRepository.FirstOrDefault(f => f.Id == folderID);
            if (folder == null)
            {
                throw new ArgumentException(L["DocumentFolderError:FolderNotExist"]);
            }

            IList<string> filePaths = DeleteFolderRecursive(projectID, userID, folderID, requireRecycle, recycleIdentity).Result;

            #region Todo
            //if (requireRecycle)
            //{
            //    List<UserRecycledEntityDataInfo> entities = new List<UserRecycledEntityDataInfo>();
            //    entities.Add(new UserRecycledEntityDataInfo()
            //    {
            //        EntityName = folder.Name,
            //        EntityClassName = "DocumentFolder",
            //        EntityKey = "ID",
            //        EntityValue = folderID.ToString(),
            //        Status = "Recycled",
            //        ProjectID = projectID,
            //        UserID = userID,
            //        CreationDate = DateTime.Now
            //    });

            //    RecycledEntityService.RecycleEntities(entities);
            //}
            #endregion

            FileUtil.DeleteFiles(filePaths);
            return true;
        }

        private async Task<IList<string>> DeleteFolderRecursive(int projectID, int userID, long parentFolderID, bool requireRecycle, Guid recycleIdentity)
        {
            List<string> filePaths = new List<string>();
            Task<List<long>> recursiveChildrenFolderIDs = GetCurrentFolderByParent(parentFolderID, new List<long>());

            // Add parentFolder
            recursiveChildrenFolderIDs.Result.Add(parentFolderID);
            IList<DocumentFolder> folders = DocumentFolderRepository.FindList(f => recursiveChildrenFolderIDs.Result.Contains(f.Id));

            /*
             * Todo
             */
            //List<UserRecycledEntityDataInfo> entities = new List<UserRecycledEntityDataInfo>();

            foreach (DocumentFolder folder in folders)
            {
                if (requireRecycle)
                {
                    /*
                     *Todo
                     */
                    //DocumentService.DeleteDocumentsOfFolderInternal(folder.Id, true, recycleIdentity);

                    folder.Status = "Recycled";
                    folder.RecycleIdentity = recycleIdentity;
                    folder.IsDeleted = true;
                    DocumentFolderRepository.UpdateAsync(folder);
                }
                else
                {
                    IList<string> paths = DeleteFolderCompletely(projectID, userID, folder);
                    filePaths.AddRange(paths);
                }
            }

            return filePaths;
        }

        private IList<string> DeleteFolderCompletely(int projectID, int userID, DocumentFolder folder)
        {
            Guid recycleIdentity = folder.RecycleIdentity.HasValue ? folder.RecycleIdentity.Value : Guid.Empty;
            IList<string> paths = DocumentService.DeleteDocumentsOfFolderInternal(projectID, userID, folder.Id, false, recycleIdentity).Result;

            //此处为旧框架操作Context的地方 新框架暂无相关操作 暂时搁置
            //IEFRepositoryContext context = this.Context as IEFRepositoryContext;
            //context.Context.Entry(folder).Collection(n => n.FolderGroupAccessControls).Load();
            //context.Context.Entry(folder).Collection(n => n.FolderUserAccessControls).Load();
            //context.Context.Entry(folder).Collection(n => n.FolderAdministrators).Load();
            DocumentFolderRepository.DeleteAsync(folder);

            return paths;
        }

        /// <summary>
        /// 获取文件夹下的子文件夹
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<long>> GetCurrentFolderByParent(long folderId, List<long> ids)
        {
            List<long> temp = new List<long>();
            List<long> list = DocumentFolderRepository.FindList(f => f.ParentFolderID == folderId && f.Status == "Created").Select(f => f.Id).ToList();

            foreach (var id in list)
            {
                ids.Add(id);
                temp.Add(id);
            }
            foreach (var item in temp)
            {
                GetCurrentFolderByParent(item, ids);
            }
            return ids;
        }


        /// <summary>
        /// 通过根文件夹ID获取该文件夹下的所有文件信息
        /// </summary>
        /// <param name="rootFolderID"></param>
        /// <returns></returns>
        public async Task<List<DocumentFolderDto>> GetAllFolders(long rootFolderID)
        {
            IList<DocumentFolder> allFolders = DocumentFolderRepository.FindList(f => f.Status == "Created");
            DocumentFolder rootFolder = DocumentFolderRepository.FindByKeyValues(rootFolderID);
            List<DocumentFolderDto> result = Create(allFolders.ToList(), rootFolder, null, false);
            return result;
        }

        private List<DocumentFolderDto> Create(List<DocumentFolder> folderList, DocumentFolder rootFolder, string suffix, bool isCount)
        {
            bool requireVerify = false;
            List<DocumentFolderDto> folders = new List<DocumentFolderDto>();
            bool parentRequireVerify = RequireVerifyByXml(rootFolder.Name);
            DocumentFolderDto rootDocumentFolderDto = new DocumentFolderDto()
            {
                Id = rootFolder.Id,
                FolderName = rootFolder.Name,
                CreateTime = rootFolder.CreationDate,
                //CreateUser = rootFolder.User.DisplayName,
                IsVerified = parentRequireVerify
            };
            folders.Add(rootDocumentFolderDto);

            List<DocumentFolderDto> childrenFolders = new List<DocumentFolderDto>();

            foreach (var item in folderList)
            {
                if (!parentRequireVerify)
                {
                    requireVerify = RequireVerifyByXml(item.Name);
                }
                else
                {
                    requireVerify = parentRequireVerify;
                }
                if (item.ParentFolderID == rootFolder.Id)
                {
                    childrenFolders.Add(new DocumentFolderDto
                    {
                        Id = item.Id,
                        FolderName = item.Name,
                        Children = GetChildren(folderList, item.Id, suffix, isCount, requireVerify),
                        Count = (isCount ? GetCountByFolderID(item.Id, suffix) : 0),
                        ParentID = item.ParentFolderID == null ? 0 :
                        item.ParentFolderID.Value,
                        CreateTime = item.CreationDate,
                        //CreateUser = item.User.DisplayName,
                        IsVerified = requireVerify
                    });
                }
            }

            rootDocumentFolderDto.Children = childrenFolders;
            if (isCount)
            {
                rootDocumentFolderDto.Count = GetCountByFolderID(rootFolder.Id, suffix);
            }

            return folders;
        }

        private bool RequireVerifyByXml(string folderName)
        {
            RequireVerifyFolderList();
            return VerifieFolderList.Contains(folderName);
        }

        private void RequireVerifyFolderList()
        {
            if (VerifieFolderList == null)
            {
                VerifieFolderList = new List<string>();
                string fileName = "VerifiedDocumentFolderDto.xml";
                string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "SystemConfiguration");
                string entityFile = Path.Combine(folderPath, fileName);
                if (File.Exists(entityFile))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        xmlDoc.Load(entityFile);
                        foreach (XmlNode folderElement in xmlDoc.SelectNodes("//Folder"))
                        {
                            if (folderElement is XmlComment)
                                continue;

                            string name = folderElement.Attributes["Name"].Value;
                            VerifieFolderList.Add(name);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        private List<DocumentFolderDto> GetChildren(List<DocumentFolder> folderList, long id, string suffix, bool isCount, bool requireVerify)
        {
            bool isVerified = false;
            List<DocumentFolderDto> fs = new List<DocumentFolderDto>();
            foreach (var item in folderList)
            {
                if (!requireVerify)
                {
                    isVerified = RequireVerifyByXml(item.Name);
                }
                else
                {
                    isVerified = requireVerify;
                }
                if (item.ParentFolderID == id)
                {
                    fs.Add(new DocumentFolderDto
                    {
                        Id = item.Id,
                        FolderName = item.Name,
                        Children = GetChildren(folderList, item.Id, suffix, isCount, isVerified),
                        Count = (isCount ? GetCountByFolderID(item.Id, suffix) : 0),
                        ParentID = item.ParentFolderID == null ? 0 : item.ParentFolderID.Value,
                        CreateTime = item.CreationDate,
                        //CreateUser = item.User.DisplayName,
                        IsVerified = isVerified
                    });
                }
            }
            return fs;
        }

        private int GetCountByFolderID(long folderId, string suffix)
        {
            Task<IList<DocumentVersion>> docVers = DocumentService.GetLatestDocVersionsByFolderInternal(folderId, suffix);
            return docVers.Result.Count();
        }
    }
}
