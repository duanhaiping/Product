using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.SharedResources.Interfaces
{
    public interface IBIMResourceManager
    {
        void InitializeResource(string resourceName, string assemblyName, CultureInfo pCultureInfo, string testResourceKey);
        string GetString(string resourceKey);
    }
}
