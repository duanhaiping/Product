using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.DocumentService;
using Microsoft.AspNetCore.Mvc;
using Platform.ToolKits.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Controllers.Document
{
    /// <summary>
    /// 文件夹相关操作方法
    /// </summary>
    public class DocumentfolderController : BaseController
    {
        private IDocumentService DocumentService { get; set; }
        private IDocumentFolderService DocumentFolderService { get; set; }
        protected bool ControlFolderPermission { get { return true; } }

        public DocumentfolderController(IDocumentService documentService, IDocumentFolderService documentFolderService)
        {
            DocumentService = documentService;
            DocumentFolderService = documentFolderService;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResult> AddFolder( long folderId, string name)
        {
            long result = DocumentFolderService.CreateFolder(folderId, name);

            if (ControlFolderPermission)
            {
                #region Todo
                //IList<UserDataInfo> user = AccessControlService.GetFolderAccessControlUserName(folderId);
                //foreach (var users in user)
                //{
                //    IList<FolderUserAccessControlDataInfo> userresult = FolderUserPermissionService.GetFolderUserAccessControl(folderId, users.ID);
                //    if (userresult.Count > 0)
                //    {
                //        FolderUserPermissionService.UpdateFolderAccessControl(userresult.Select(n => n.AccessControlID).ToList(), result, userresult[0].UserID);
                //    }
                //}
                //IList<GroupDataInfo> group = AccessControlService.GetFolderAccessControlGroupName(folderId);
                //foreach (var groups in group)
                //{
                //    IList<FolderGroupAccessControlDataInfo> groupresult = FolderGroupAccessControlService.GetFolderGroupAccessControl(folderId, groups.ID);
                //    if (groupresult.Count > 0)
                //    {
                //        FolderGroupAccessControlService.AddFolderGroupAccessControl(groupresult.Select(n => n.AccessControlID).ToList(), result, groupresult[0].GroupID);
                //    }
                //}
                //IList<FolderAdministratorDataInfo> folderadmin = FolderAdministratorService.GetFolderAdministrator(folderId);
                //foreach (var item in folderadmin)
                //{
                //    FolderAdministratorService.RegisterFolderAdministrator(new FolderAdministratorDataInfo { UserID = item.UserID, FolderID = result });
                //}
                #endregion
            }

            return await ServiceResult.IsSuccess("创建成功");
        }

        /// <summary>
        /// 修改文件夹名称
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResult> RenameFolderName([FromBody] long folderID, string name)
        {
            bool result = DocumentFolderService.RenameFolderName(folderID, name);
            return await ServiceResult.IsSuccess("修改成功");
        }

        /// <summary>
        /// 删除文件夹及其子文件夹和文件
        /// </summary>
        /// <param name="folderID"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ServiceResult> RemoveFolder([FromRoute] long folderID)
        {
            bool result = DocumentFolderService.DeleteFolder(CurrentProject, CurrentUser.Id.Value, folderID, true, Guid.NewGuid());
            return await ServiceResult.IsSuccess("删除成功");
        }

        /// <summary>
        /// 获取当前项目下的文件夹结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResult> GetFolderStructure()
        {
            FolderDto rootFolder = DocumentFolderService.GetProjectRootFolder(this.CurrentProject);
            if (rootFolder == null)
            {
                return await ServiceResult<IList<FolderStructure>>.PageList(null, 0, "未找到对应的文件夹信息");
            }

            IList<FolderStructure> list = DocumentFolderService.GetFolderStructure(rootFolder.ID, null, CurrentUser.Id.Value);
            return await ServiceResult<IList<FolderStructure>>.PageList(list, list.Count, string.Empty);
        }
    }
}
  