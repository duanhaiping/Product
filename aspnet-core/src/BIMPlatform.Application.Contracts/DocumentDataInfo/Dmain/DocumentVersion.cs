
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BIMPlatform.Application.Contracts.UserDataInfo;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo.Domain
{
    public class DocumentVersion
    {
        private long mFileLength = 0;
        private string mSize = "0";
        private string statusStr = "";
        //private Document mobjParentDocument = null;
        private UserDto mobjCreationUserInfo;

        //public Document ParentDocument
        //{
        //    get { return mobjParentDocument; }
        //    set
        //    {
        //        mobjParentDocument = value;
        //        DocumentID = mobjParentDocument.ID;
        //        Name = mobjParentDocument.Name;
        //        DocNumber = mobjParentDocument.DocNumber;
        //        Suffix = mobjParentDocument.Suffix;
        //    }
        //}

        public long ID { get; set; }

        public long FolderID { get; set; }

        public long DocumentID { get; set; }

        public string Name { get; set; }

        public string DocNumber { get; set; }
        public int Version { get; set; }
        public string MD5 { get; set; }
        public string Suffix { get; set; }
        public string Status { get; set; }

        public long FileLength
        {
            get
            {
                return mFileLength;
            }
            set
            {
                mFileLength = value;
                mSize = mFileLength.ToString();
            }
        }

        // For saving into DB
        public string Size
        {
            get { return mSize; }
            set
            {
                mSize = value;
                long.TryParse(mSize, out mFileLength);
            }
        }

        // Format as display
        public string SizeDisplay { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Properties { get; set; }
        public string RemotePath { get; set; }
        public int CreationUserID { get; set; }
        public string ParentFolderName { get; set; }
        public string CreationUser { get; set; }

        public string StatusStr { get; set; }

        public UserDto CreationUserInfo
        {
            get
            {
                return mobjCreationUserInfo;
            }
            set
            {
                mobjCreationUserInfo = value;
                CreationUser = mobjCreationUserInfo.DisplayName;
                CreationUserID = mobjCreationUserInfo.ID;
            }
        }

        public DocumentVersion()
        {

        }

        // Not allow another constructor for AutoMapper?
        //
        //public DocumentVersion(Document document)
        //{
        //    mobjParentDocument = document;
        //    DocumentID = mobjParentDocument.ID;
        //}

        public override bool Equals(object obj)
        {
            if (obj is DocumentVersion)
            {
                return ((DocumentVersion)obj).ID == this.ID;
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
