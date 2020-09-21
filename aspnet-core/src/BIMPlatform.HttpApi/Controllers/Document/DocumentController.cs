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
        ///// <summary>
        ///// 上传文件
        ///// </summary>
        ///// <param name="uploadParams"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[SwaggerResponse(200, "上传成功", null)]
        //public async Task<ServiceResult> Upload([FromForm] DocumentUploadParams uploadParams)
        //{
        //    for (int i = 0; i < uploadParams.Files.Count; i++)
        //    {
        //        DocumentDto document = new DocumentDto() {
        //            FolderID = uploadParams.FolderID,
        //            Name = uploadParams.Files[i].FileName,
        //            Suffix = Path.GetExtension(uploadParams.Files[i].FileName),
        //            Stream = uploadParams.Files[i].OpenReadStream(),
        //            RequireTransform = true,
        //            SupportDocNumber = true,
        //            ClientRelativePath = uploadParams.ClientRelativePath,
        //        };

        //        await DocumentService.UploadFile(document);
        //    }
        //    return await ServiceResult.IsSuccess();
        //}

        ///// <summary>
        ///// 创建文件夹
        ///// </summary>
        ///// <param name="folderId"></param>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<ServiceResult> AddFolder([FromQuery] long folderId, string name)
        //{
        //    //创建文件夹
        //    long result = DocumentFolderService.CreateFolder(folderId, name);

        //    #region Todo
        //    //if (ControlFolderPermission)
        //    //{
        //    //    IList<UserDataInfo> user = AccessControlService.GetFolderAccessControlUserName(folderId);
        //    //    foreach (var users in user)
        //    //    {
        //    //        IList<FolderUserAccessControlDataInfo> userresult = FolderUserPermissionService.GetFolderUserAccessControl(folderId, users.ID);
        //    //        if (userresult.Count > 0)
        //    //        {
        //    //            FolderUserPermissionService.UpdateFolderAccessControl(userresult.Select(n => n.AccessControlID).ToList(), result, userresult[0].UserID);
        //    //        }
        //    //    }
        //    //    IList<GroupDataInfo> group = AccessControlService.GetFolderAccessControlGroupName(folderId);
        //    //    foreach (var groups in group)
        //    //    {
        //    //        IList<FolderGroupAccessControlDataInfo> groupresult = FolderGroupAccessControlService.GetFolderGroupAccessControl(folderId, groups.ID);
        //    //        if (groupresult.Count > 0)
        //    //        {
        //    //            FolderGroupAccessControlService.AddFolderGroupAccessControl(groupresult.Select(n => n.AccessControlID).ToList(), result, groupresult[0].GroupID);
        //    //        }
        //    //    }
        //    //    IList<FolderAdministratorDataInfo> folderadmin = FolderAdministratorService.GetFolderAdministrator(folderId);
        //    //    foreach (var item in folderadmin)
        //    //    {
        //    //        FolderAdministratorService.RegisterFolderAdministrator(new FolderAdministratorDataInfo { UserID = item.UserID, FolderID = result });
        //    //    }
        //    //}
        //    #endregion

        //    return await ServiceResult.IsSuccess();
        //}

        ///// <summary>
        ///// 修改文件夹名称
        ///// </summary>
        ///// <param name="folderID"></param>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<ServiceResult> RenameFolderName([FromQuery] long folderID, string name)
        //{
        //    bool result = DocumentFolderService.RenameFolderName(folderID, name);
        //    return await ServiceResult.IsSuccess();
        //}

        ///// <summary>
        ///// 删除文件夹及其子文件夹和文件
        ///// </summary>
        ///// <param name="folderID"></param>
        ///// <returns></returns>
        //[HttpDelete]
        ////[APIPermissionFilter(SystemName = "DeleteDocumentFolder", Type = PermissionType.Project)]
        //public async Task<ServiceResult> RemoveFolder([FromQuery] long folderID)
        //{
        //    bool result = DocumentFolderService.DeleteFolder(folderID, true, Guid.NewGuid());
        //    return await ServiceResult.IsSuccess();
        //}
    }
}
