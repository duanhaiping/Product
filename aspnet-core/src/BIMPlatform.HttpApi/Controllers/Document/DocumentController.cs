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
    /// 
    /// </summary>
    public class DocumentController : BaseController
    {
        private IDocumentService DocumentService { get; set; }

        public DocumentController(IDocumentService documentService)
        {
            DocumentService = documentService;
        }
        /// <summary>
        /// ww
        /// </summary>
        /// <param name="uploadParams"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200, "", null)]
        public async Task<ServiceResult> Upload([FromForm]DocumentUploadParams uploadParams)
        {
            await DocumentService.UploadAsync(uploadParams);
            return await ServiceResult.IsSuccess();
        }
    }
}
