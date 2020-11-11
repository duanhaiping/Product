using System;
using System.Collections.Generic;
using System.Text;

namespace BIMPlatform
{
    public static class ApiGrouping
    {
        /// <summary>
        /// Abp vnext框架自带接口
        /// </summary>
        public const string GroupName_v1 = "v1";

        /// <summary>
        /// 平台管理角色使用的接口
        /// </summary>
        public const string GroupName_v2 = "v2";

        /// <summary>
        /// 租户系统模块-企业级系统管理模块接口
        /// </summary>
        public const string GroupName_v3 = "v3";

        /// <summary>
        /// 租户项目模块接口--企业级项目管理模块接口
        /// </summary>
        public const string GroupName_v4 = "v4";
        #region 第三方接口区域
        /// <summary>
        /// 钉钉
        /// </summary>
        public const string GroupName_v5 = "v5";
        /// <summary>
        /// 微信
        /// </summary>
        public const string GroupName_v6 = "v6";
        /// <summary>
        /// QQ
        /// </summary>
        public const string GroupName_v7 = "v7";

        /// <summary>
        /// 未定义分组
        /// </summary>
        public const string GroupName_v999 = "v999";
        #endregion
    }
}
