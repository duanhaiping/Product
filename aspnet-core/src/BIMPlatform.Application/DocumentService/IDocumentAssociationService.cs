using BIMPlatform.Application.Contracts.Document;
using BIMPlatform.Application.Contracts.DocumentDataInfo;
using BIMPlatform.Application.Contracts.Entity;
using BIMPlatform.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.DocumentService
{
    public interface IDocumentAssociationService
    {
        /// <summary>
        /// Get all associated documents for entity by document type
        /// </summary>
        /// <param name="entityInfo"></param>
        /// <param name="docType"></param>
        /// <returns></returns>
        IList<DocumentAssociationDataInfo> GetAllAssociatedDocumentsByEntity(EntityDataInfo entityInfo, string docType = "", bool getFileStream = false);

        IList<EntityDocument> GetRelatedDocumentRecordByEntity(EntityDataInfo entityInfo, string docType = "");

        IList<EntityDocument> GetRelatedDocumentRecordBySameTypeEntities(string entityClassName, string entityKey, IEnumerable<string> entityValues, string docType = "");

        /// <summary>
        /// Asscociated a list of document versions to entity
        /// </summary>
        /// <param name="entityInfo"></param>
        /// <param name="docVersions"></param>
        /// <param name="docType"></param>
        /// <param name="autoRevised"></param>
        /// <param name="temporary"></param>
        void AssociateDocuments(AssociationEntityDataInfo entityInfo, IEnumerable<DocumentVersionDto> docVersions, string docType, bool autoRevised, bool temporary);

        /// <summary>
        /// Asscociated a list of document versions to entity
        /// </summary>
        /// <param name="entityInfo"></param>
        /// <param name="docVersions"></param>
        /// <param name="docType"></param>
        /// <param name="autoRevised"></param>
        /// <param name="temporary"></param>
        void AssociateDocumentsInternal(AssociationEntityDataInfo entityInfo, IEnumerable<DocumentVersionDto> docVersions, string docType, bool autoRevised);

        void AssociateCommentedDocumentsInternal(AssociationEntityDataInfo entityInfo, IEnumerable<CommentedDocumentVersion> docVersions, string docType, bool autoRevised);

        /// <summary>
        /// Asscociated a list of document versions to entity
        /// </summary>
        /// <param name="entityInfo"></param>
        /// <param name="docVersionIDs"></param>
        /// <param name="docType"></param>
        /// <param name="autoRevised"></param>
        void AssociateDocuments(AssociationEntityDataInfo entityInfo, IEnumerable<long> docVersionIDs, string docType, bool autoRevised);

        /// <summary>
        /// Remove association documents from entity
        /// </summary>
        /// <param name="entityInfo"></param>
        /// <param name="docVersionIDs"></param>
        /// <param name="docType"></param>
        void RemoveDocumentAssociation(EntityDataInfo entityInfo, IEnumerable<long> docVersionIDs, string docType);

        IList<object> GetAssociatedEntities(long documentVersionID, string docType = "");

        void ValidatePreview(IEnumerable<DocumentVersionDto> docVersions);

        void AssociateDocument(AssociationEntityDataInfo entityInfo, DocumentVersionDto docVersion, string docType, bool autoRevised);
        void RemoveAssociationByDocVersionIDsInternal(IEnumerable<long> docVersionIDs);
        void RemoveAssociationBySameEntities(EntityDataInfo entityInfo, IEnumerable<string> entityValues, string docType = "");

        IList<EntityDocument> GetRelatedDocumentRecordByDescription(EntityDataInfo entityInfo, string description, string docType = "");
        void GetAssociateDocument(AssociationEntityDataInfo entityInfo, DocumentVersionDto docVersion, string docType, bool autoRevised);
    }
}
