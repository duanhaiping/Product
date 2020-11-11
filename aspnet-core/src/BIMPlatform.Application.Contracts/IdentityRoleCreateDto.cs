using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts
{
    public class IdentityRoleCreateDto :Volo.Abp.Identity.IdentityRoleCreateDto
    {
        public string RoleType { get; set; }
    }
}
