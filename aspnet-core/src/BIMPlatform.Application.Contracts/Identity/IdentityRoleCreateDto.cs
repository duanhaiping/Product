using BIMPlatform.ToolKits.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BIMPlatform.Application.Contracts.Identity
{
    public  class BIMIdentityRoleCreateDto 
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsPublic { get; set; }
        public RoleTypeEnum RoleType { get; set; }
    }
  

}
