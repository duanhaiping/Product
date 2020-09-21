
using BIMPlatform.Document;
using BIMPlatform.DocumentService;
using BIMPlatform.Repositories.Document;
using log4net.Repository.Hierarchy;
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

        private List<string> VerifieFolderList { get; set; }

        public DocumentFolderCommonService(IDataFilter dataFilter, IDocumentFolderRepository documentFolderRepository)
        {
            DataFilter = dataFilter;
            DocumentFolderRepository = documentFolderRepository;
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
    }
}
