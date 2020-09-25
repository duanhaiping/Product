
using BIMPlatform.Document;
using BIMPlatform.DocumentService;
using BIMPlatform.Repositories.Document;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace BIMPlatform.DocumentService.impl
{
    public partial class DocumentFolderCommonService : BaseService, IDocumentFolderCommonService
    {
        private readonly IDataFilter DataFilter;
        private readonly IDocumentFolderRepository DocumentFolderRepository;
        private readonly IDocumentRepository DocumentRepository;

        private List<string> VerifieFolderList { get; set; }

        public DocumentFolderCommonService(IDataFilter dataFilter, IDocumentFolderRepository documentFolderRepository, IDocumentRepository documentRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            DataFilter = dataFilter;
            DocumentFolderRepository = documentFolderRepository;
            DocumentRepository = documentRepository;
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
            DocumentFolder folder = CreateFolderEntity(parentFolderID, folderName);
            return folder.Id;
        }

        /// <summary>
        /// 创建文件夹实体（操作数据库）
        /// </summary>
        /// <param name="parentFolderID"></param>
        /// <param name="folderName"></param>
        /// <param name="creationUserID"></param>
        /// <returns></returns>
        public DocumentFolder CreateFolderEntity(long? parentFolderID, string folderName)
        {
            DocumentFolder parentfolder = null;
            DocumentFolder existingFolder = null;

            if (folderName == null)
            {
                throw new ArgumentException(L["DocumentFolderError:NullName"]);
            }

            if (parentFolderID.HasValue)
            {
                parentfolder = DocumentFolderRepository.FirstOrDefault(n => n.Id == parentFolderID.Value);
                //if (parentfolder == null)
                //{
                //    throw new ArgumentException(L["DocumentFolderError:FolderNotExist"]);
                //}

                existingFolder = DocumentFolderRepository.FirstOrDefault(f => f.ParentFolderID == parentFolderID.Value && f.Name == folderName && f.Status == "Created");
            }
            else
            {
                existingFolder = DocumentFolderRepository.FirstOrDefault(f => f.ParentFolderID == null && f.Name == folderName && f.Status == "Created");
            }

            if (existingFolder != null)
            {
                throw new ArgumentException(L["DocumentFolderError:DuplicateFolderName"]);
            }

            DocumentFolder folder = new DocumentFolder();
            folder.CreationDate = DateTime.Now;
            folder.Name = folderName;
            folder.Status = "Created";

            if (parentfolder != null)
            {
                folder.ParentFolderID = parentfolder.Id;
            };

            DocumentFolderRepository.InsertAsync(folder);
            return folder;
        }

        public DocumentFolder GetOrCreateFolderByPathInternal(DocumentFolder parentFolder, int creationUserID, string folderPath)
        {
            DocumentFolder targetFolder = parentFolder;

            string[] folderArray = folderPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string folder in folderArray)
            {
                parentFolder = BuildFolder(parentFolder, folder, creationUserID);
            }
            return parentFolder;
        }

        private DocumentFolder BuildFolder(DocumentFolder parentFolder, string folderName, int creationUserID)
        {
            DocumentFolder folder = DocumentFolderRepository.FirstOrDefault(f => f.ParentFolderID == parentFolder.Id && f.Name == folderName);
            if (folder == null)
            {
                folder = new DocumentFolder()
                {
                    CreationDate = DateTime.Now,
                    Name = folderName,
                    Status = "Created",
                    ParentFolderID = parentFolder.Id,
                    CreationUserID = creationUserID,
                };
                DocumentFolderRepository.Add(folder);
            }
            return folder;
        }

        public bool CanCopyDocuments(long targetFolderID, List<long> documentIDs, out string error)
        {
            bool result = true;
            error = string.Empty;

            if (HasSameParentFolder(targetFolderID, documentIDs))
            {
                result = false;
                //error = ResourceManager.GetString("Document_CopyDoc_SameParentFolderError");
            }

            return result;
        }

        private bool HasSameParentFolder(long targetFolderID, List<long> documentIDs)
        {
            IList<Document.Document> docs = DocumentRepository.FindList(d => documentIDs.Contains(d.Id) && d.FolderID == targetFolderID);
            return docs != null && docs.Count > 0;
        }

        public  bool CanMoveDocuments(long targetFolderID, List<long> documentIDs, out string error)
        {
            bool result = true;
            error = string.Empty;

            if (HasSameParentFolder(targetFolderID, documentIDs))
            {
                result = false;
                //error = ArgumentException(L["Document_MoveDoc_SameParentFolderError"]);
            }

            return result;
        }
    }
}
