using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BIMPlatform.Application.Contracts.UserDataInfo
{
    /// <summary>
    /// 用户
    /// </summary>
    /// 
    public class UserDto
    {
        public int ID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        public string UserHeadImgUrl { get; set; }

        public string WeiXinOpenID { get; set; }

        public int? GroupID { get; set; }

        public string GroupName { get; set; }

        public bool IsActivated { get; set; }

        public long? OrganizationID { get; set; }

        public bool IsSystemAdministrator { get; set; }
        public bool IsEmployee { get; set; }
        public string IDCard { get; set; }
        public string CardNo { get; set; }
        public Nullable<long> CompanyID { get; set; }
        public Nullable<long> CraftID { get; set; }
        public Nullable<long> AddressID { get; set; }

        public string OrganizationName { get; set; }

        //public IList<RoleDataInfo> UserRoles { get; set; }

        public IList<int> UserRoleIDs { get; set; }
        /// <summary>
        /// 所持角色
        /// </summary>
        //public IList<RoleDataInfo> MyAllProjectRoles { get; set; }

        //protected override void StoreUnchangedFieldsForUpdate(User existingEntity)
        //{

        //    if (string.IsNullOrEmpty(this.Password))
        //    {
        //        this.Password = existingEntity.Password;
        //    }
        //    else
        //    {
        //        this.Password = EncrypManager.Encode(this.Password);
        //    }
        //    if (string.IsNullOrEmpty(this.DisplayName))
        //    {
        //        this.DisplayName = existingEntity.DisplayName;
        //    }
        //    else
        //    {
        //        this.DisplayName = this.DisplayName;
        //    }
        //    if (string.IsNullOrEmpty(this.Name))
        //    {
        //        this.Name = existingEntity.Name;
        //    }
        //    else
        //    {
        //        this.Name = this.Name;
        //    }
        //    this.OrganizationID = existingEntity.OrganizationID;
        //    this.IsActivated = existingEntity.IsActivated;
        //}
    }
}
