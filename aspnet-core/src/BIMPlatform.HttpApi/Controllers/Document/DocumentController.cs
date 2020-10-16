using BIMPlatform.Application.Contracts;
using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Application.Contracts.ProjectDataInfo;
using BIMPlatform.DocumentService;
using BIMPlatform.ProjectDataInfo;
using BIMPlatform.ProjectService;
using Microsoft.AspNetCore.Mvc;
using Platform.ToolKits.Base;
using Platform.ToolKits.Base.Enum;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Controllers.Project
{
    /// <summary>
    /// 文件类操作方法
    /// </summary>
    public class DocumentController : BaseController
    {
        private IDocumentService DocumentService { get; set; }
        private IDocumentFolderService DocumentFolderService { get; set; }

        public DocumentController(IDocumentService documentService)
        {
            DocumentService = documentService;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadParams"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200, "上传成功", null)]
        public async Task<ServiceResult> UploadFile([FromForm] DocumentUploadParams uploadParams)
        {
            for (int i = 0; i < uploadParams.Files.Count; i++)
            {
                DocumentFileDataInfo document = new DocumentFileDataInfo()
                {
                    FolderID = uploadParams.CurrentNode,
                    Name = uploadParams.Files[i].FileName,
                    Suffix = Path.GetExtension(uploadParams.Files[i].FileName),
                    Stream = uploadParams.Files[i].OpenReadStream(),
                    RequireTransform = true,
                    SupportDocNumber = true,
                    ClientRelativePath = uploadParams.ClientRelativePath,
                    ProjectID=CurrentProject
                };

                await DocumentService.UploadFile(CurrentProject,document);
            }
            return await ServiceResult.IsSuccess();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="docVersionIDs"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResult>  DownloadFiles([FromQuery]string docVersionIDs)
        {
            List<long> versionIDs = new List<long>();
            foreach (var id in docVersionIDs.Split(','))
            {
                long versionID = 0;
                if (long.TryParse(id, out versionID))
                {
                    versionIDs.Add(versionID);
                }
            }

            DownloadFileItemDataInfo fileItem = DocumentService.DownloadFiles(versionIDs, CurrentUser.Id.Value);
            return await ServiceResult< DownloadFileItemDataInfo >.IsSuccess(fileItem) ;
        }


        /// <summary>
        /// 下载文件夹
        /// 下载文件后，将生成的临时文件删除
        /// </summary>
        /// <param name="downloadInfopath"></param>
        /// <param name="downloadInfoname"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DownloadFilesOfFolder([FromQuery]string downloadInfopath, string downloadInfoname)
        {
            // Depends on client whether download sub folders
            //DownloadFolderItemDataInfo downloadInfo = DocumentService.DownloadFilesOfFolder(folderID, CurrentUser, true);
            HttpResponseMessage result = null;
            if (!string.IsNullOrEmpty(downloadInfopath))
            {
                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(downloadInfopath, FileMode.Open));
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = downloadInfoname
                };
            }
            else
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return result;
        }

        /// <summary>
        /// 删除文件(需要回收)
        /// </summary>
        /// <param name="docID"></param>
        /// <returns></returns>
        [HttpDelete]
        public Task<ServiceResult> RemoveDocument([FromQuery]int docID)
        {
            DocumentService.DeleteDocumentByVersionID(CurrentProject, CurrentUser.Id.Value, docID, true, Guid.NewGuid());
            return ServiceResult.IsSuccess();
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="folderID">target folder id</param>
        /// <param name="docIDs">document id </param>
        /// <returns></returns>
        [HttpGet]
        public Task<ServiceResult> CopyDocumentsToFolder([FromQuery]long folderID, string docIDs)
        {
            List<long> docIDList = docIDs.Split(',').Select(long.Parse).ToList();
            DocumentService.CopyDocumentsToFolderAsync(CurrentProject, folderID, docIDList, CurrentUser.Id.Value);
            return ServiceResult.IsSuccess();
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="docIDs"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<ServiceResult> MoveDocumentsToFolder([FromQuery]long folderID, string docIDs)
        {
            List<long> docIDList = docIDs.Split(',').Select(long.Parse).ToList();
            DocumentService.MoveDocumentsToFolderAsync(CurrentProject, folderID, docIDList, CurrentUser.Id.Value);
            return ServiceResult.IsSuccess();
        }
    }
}
