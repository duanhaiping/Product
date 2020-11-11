using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.Group
{
    public class GroupDataInfo
    {
        public int ID { get; set; }
        public int? ParentGroupID { get; set; }
        public string Name { get; set; }
        public string ParentGroupName { get; set; }
        public string FullName { get; set; }
        public string Abbreviation { get; set; }
        public string PrimaryCategory { get; set; }
        public string SecondaryCategory { get; set; }
        public string Description { get; set; }
        public bool IsActivated { get; set; }
        public long OrganizationId { get; set; }
    }
}
