namespace BIMPlatform.Permissions
{
    public static class BIMPlatformPermissions
    {
        public const string GroupName = "BIMPlatform";

        /// <summary>
        /// 定义了部分通用的默认接口类型，若通用类型中存在，则使用通用接口类型，不存在则在内部类模块中自己添加
        /// </summary>
        private class DefaultPermissionType
        {
            public const string Default = ".Default"; //查看
            
            public const string Create = ".Create";
            public const string Update = ".Update";
            public const string Delete = ".Delete";
            public const string Manage = ".Manage"; // 管理 = 查看+ 增加+更新+ 删除+审核+审批+
            public const string Audit = ".Audit"; //审核是审查和核实，有时无最终决定权；
            public const string Approve = ".Approve"; //审批是审查和批准。随时有最终决定权。
            public const string Rectify = ".Rectify";
           // public const string Apply = ".Apply";
        }
        public class Project
        {
            private const string ModuleName = ".Project";
            public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
            public const string Create= GroupName + ModuleName + DefaultPermissionType.Create;
            public const string Update = GroupName + ModuleName + DefaultPermissionType.Update;
            public const string Delete = GroupName + ModuleName + DefaultPermissionType.Delete;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }
        #region abp 系统已内置模块
        // abp 模块中已存在租户相关的权限，所以暂时不需要租户相关权限，如有需要，尽量在原有基础上进行扩展
        //public class Tenant
        //{
        //    private const string ModuleName = ".Tenant";
        //    public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
        //    public const string Create = GroupName + ModuleName + DefaultPermissionType.Create;
        //    public const string Update = GroupName + ModuleName + DefaultPermissionType.Update;
        //    public const string Delete = GroupName + ModuleName + DefaultPermissionType.Delete;
        //}

        // abp 模块中已存在用户相关的权限，具体为 Identity management ，所以暂时不需要定义，如有需要，尽量在原有基础上进行扩展
        //public class User
        //{
        //    private const string ModuleName = ".User";

        //    public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
        //    public const string Create = GroupName + ModuleName + DefaultPermissionType.Create;
        //    public const string Update = GroupName + ModuleName + DefaultPermissionType.Update;
        //    public const string Delete = GroupName + ModuleName + DefaultPermissionType.Delete;
        //    public const string AssignUserRole = GroupName + ModuleName + ".AssignUserRole";
        //}
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public class Group 
        {
            private const string ModuleName = ".Group";
            public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
            public const string Create = GroupName + ModuleName + DefaultPermissionType.Create;
            public const string Update = GroupName + ModuleName + DefaultPermissionType.Update;
            public const string Delete = GroupName + ModuleName + DefaultPermissionType.Delete;
            public const string AssignGroup = GroupName + ModuleName + ".AssignGroupToProject";
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }
        public class Attendance
        {
            private const string ModuleName = ".Attendance";
            public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
            public const string ViewLeave = GroupName + ModuleName +".Leave" + DefaultPermissionType.Default;
            public const string ApproveLeave = GroupName + ModuleName + ".Leave" + DefaultPermissionType.Approve;
            public const string ViewSheWolf = GroupName + ModuleName + ".SheWolf" + DefaultPermissionType.Default;
            public const string ManageSheWolf = GroupName + ModuleName + ".SheWolf"+ DefaultPermissionType.Manage;
            public const string RealTimeStatistics = GroupName + ModuleName + ".RealTimeStatistics" + DefaultPermissionType.Default;
            public const string WeekAndMounthStatistics = GroupName + ModuleName + ".WeekAndMounthStatistics" + DefaultPermissionType.Default;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }
        public class EfficiencyAnalysis
        {
            private const string ModuleName = ".EfficiencyAnalysis";
            public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }

        public class Equipment
        {
            private const string ModuleName = ".Equipment";
            public const string ViewTowerCrane = GroupName + ModuleName + ".TowerCrane" +DefaultPermissionType.Default;
            public const string ViewEnvironment = GroupName + ModuleName + ".Environment" + DefaultPermissionType.Default;
            public const string ViewVideo = GroupName + ModuleName + ".Video" + DefaultPermissionType.Default;
            public const string ManageVideo=GroupName + ModuleName + ".Video" + DefaultPermissionType.Manage;
            public const string ViewElevator = GroupName + ModuleName + ".Elevator" + DefaultPermissionType.Default;
            public const string ViewCar = GroupName + ModuleName + ".Car" + DefaultPermissionType.Default;
            public const string ViewHighModulus = GroupName + ModuleName + ".HighModulus" + DefaultPermissionType.Default;
            public const string ViewDeepExcavation = GroupName + ModuleName + ".DeepExcavation" + DefaultPermissionType.Default;
            public const string ViewLoadometer = GroupName + ModuleName + ".Loadometer" + DefaultPermissionType.Default;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }
        public class Document
        {
            private const string ModuleName = ".Document";
            public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
            public const string Create = GroupName + ModuleName + DefaultPermissionType.Create;
            public const string Update = GroupName + ModuleName + DefaultPermissionType.Update;
            public const string Delete = GroupName + ModuleName + DefaultPermissionType.Delete;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }

        public class KnowledgeBase
        {
            private const string ModuleName = ".KnowledgeBase";
            public const string ViewQA = GroupName + ModuleName +".QA"+ DefaultPermissionType.Default;
            public const string ViewRegulation = GroupName + ModuleName + ".Regulation" + DefaultPermissionType.Default;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
           
        }

        public class SafetyIssue
        {
            private const string ModuleName = ".SafetyIssue";
            public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
            public const string Create = GroupName + ModuleName + DefaultPermissionType.Create;
            public const string Update = GroupName + ModuleName + DefaultPermissionType.Update;
            public const string Delete = GroupName + ModuleName + DefaultPermissionType.Delete;
            public const string Rectify = GroupName + ModuleName + DefaultPermissionType.Rectify;
            public const string Audit = GroupName + ModuleName + DefaultPermissionType.Audit;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }

        public class QualityIssue
        {
            private const string ModuleName = ".QualityIssue";
            public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
            public const string Create = GroupName + ModuleName + DefaultPermissionType.Create;
            public const string Update = GroupName + ModuleName + DefaultPermissionType.Update;
            public const string Delete = GroupName + ModuleName + DefaultPermissionType.Delete;
            public const string Rectify = GroupName + ModuleName + DefaultPermissionType.Rectify;
            public const string Audit = GroupName + ModuleName + DefaultPermissionType.Audit;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }
        public class SupervisorLog
        {
            private const string ModuleName = ".SupervisorLog";
            public const string Default = GroupName + ModuleName + DefaultPermissionType.Default;
            public const string Create = GroupName + ModuleName + DefaultPermissionType.Create;
            public const string Update = GroupName + ModuleName + DefaultPermissionType.Update;
            public const string Delete = GroupName + ModuleName + DefaultPermissionType.Delete;
            public const string Manage = GroupName + ModuleName + DefaultPermissionType.Manage;
        }
    }
}
       
