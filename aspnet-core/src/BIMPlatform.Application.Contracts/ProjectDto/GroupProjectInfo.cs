using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.ProjectDto
{
    public class GroupProjectInfo
    {
        public int GroupID { get; set; }
        public int ProjectID { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
