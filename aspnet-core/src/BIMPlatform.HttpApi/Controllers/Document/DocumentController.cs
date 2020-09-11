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
        public async Task<ServiceResult> Upload([FromForm]DocumentUploadParams uploadParams)
        {
            await DocumentService.UploadAsync(uploadParams);
            return await ServiceResult.IsSuccess();
        }
    }
}
