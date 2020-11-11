using BIMPlatform.Application.Contracts.DocumentDataInfo;

namespace BIMPlatform.Application.Contracts.Entity
{
    public class DocumentAssociationDataInfo 
    {
        public string EntityName { get; set; }
        public string EntityKey { get; set; }
        public string EntityValue { get; set; }
        public long DocVersionID { get; set; }
        public string Type { get; set; }
        public int CreationUserID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool AutoRevised { get; set; }
        public bool DocumentChanged { get; set; }
        public string Description { get; set; }
        public DocumentVersionDto AssociatedDocVersion { get; set; }
    }
}
