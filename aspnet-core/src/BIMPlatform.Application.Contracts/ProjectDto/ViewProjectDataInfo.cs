using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.ProjectDto
{
    public class ViewProjectDataInfo
    {
        public bool ViewAllProjects { get; set; }
        public bool ViewOwnedProjects { get; set; }

        public ViewProjectDataInfo()
        {
            ViewAllProjects = false;
            ViewOwnedProjects = false;
        }
    }
}
