using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPlatform.Document;

namespace BIMPlatform.Module.Document.Domain
{
    public class Folder
    {
        public const string SharedType = "ProjectShared";
        public const string ProjectType = "Project";
        //public const string ModelSharedType = "ProjectModelShared";

        public long ID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public long? ParentFolderID { get; set; }
        public int CreationUserID { get; set; }
        public string Status { get; set; }

        public bool IsProjectRootFolder { get; set; }
        public bool IsProjectSharedFolder { get; set; }

        public List<BIMPlatform.Document.Document> Documents { get; set; }

        public Folder()
        {
            Documents = new List<BIMPlatform.Document.Document>();
        }

        public BIMPlatform.Document.Document GetDocumentByName(string name)
        {
            if (Documents != null && Documents.Count > 0)
                return Documents.FirstOrDefault(doc => string.Compare(doc.Name, name, true) == 0);
            else
                return null;
        }

        public bool HasDuplicateDocument(string name)
        {
            BIMPlatform.Document.Document doc = GetDocumentByName(name);
            return doc != null;
        }

        public override bool Equals(object obj)
        {
            if (obj is Folder)
            {
                return ((Folder)obj).ID == this.ID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
 