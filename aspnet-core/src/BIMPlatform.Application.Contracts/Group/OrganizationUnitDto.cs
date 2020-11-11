using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform.Application.Contracts.Group
{
    public class OrganizationUnitEto
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }
    }
}
