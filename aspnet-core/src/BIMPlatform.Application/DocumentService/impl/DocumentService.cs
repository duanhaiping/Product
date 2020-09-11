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

namespace BIMPlatform.DocumentService.impl
{
    public partial class DocumentService : BaseService, IDocumentService
    {
        private readonly IDocumentRepository DocumentRepository;
        private readonly IDataFilter DataFilter;

        public DocumentService(IDocumentRepository documentRepository, IDataFilter dataFilter)
        {
            DataFilter = dataFilter;
            DocumentRepository = documentRepository;
        }

        public async Task UploadAsync(DocumentUploadParams uploadParams)
        {
            Document.Document document = ObjectMapper.Map<DocumentUploadParams, Document.Document>(uploadParams);
            document.Suffix = uploadParams.File.FileName.Split('.').Count() > 1 ? "." + uploadParams.File.FileName.Split('.')[1].ToLower() : string.Empty;
            document.Status= string.IsNullOrEmpty(uploadParams.Status.ToString())? "Created" : uploadParams.Status.ToString();
            document.CreationTime = DateTime.Now;
            document.CreatorId = CurrentUser.Id;
            document.TenantId = CurrentTenant.Id;
            document.IsDeleted = false;

            await DocumentRepository.InsertAsync(document);
            
        }
    }
}
