using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Application.Contracts.ProjectDataInfo;
using BIMPlatform.Document;
using BIMPlatform.ProjectDataInfo;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace BIMPlatform.DocumentService
{
    public  partial interface IDocumentService
    {
        Task UploadAsync(DocumentUploadParams documentFile);
    }
}
