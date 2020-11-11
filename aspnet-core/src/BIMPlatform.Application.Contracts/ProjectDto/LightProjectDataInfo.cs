using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.ProjectDto
{
    public class LightProjectDataInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int CreationUserID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Address { get; set; }
        public decimal ProjectEstimate { get; set; }
       // public IList<RoleDataInfo> ProjectRoles { get; set; }
    }
}
