using BIMPlatform.Localization;
using System.Collections.Generic;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace BIMPlatform.Permissions
{
    public class BIMPlatformPermissionDefinitionProvider : PermissionDefinitionProvider
    {
   
        public override void Define(IPermissionDefinitionContext context)
        {
            var projectGroup = context.AddGroup(BIMPlatformPermissionGroupDefinition.ProjectGroup, L("Permission:Project"),MultiTenancySides.Tenant);

            #region project
            var projectPermissions = projectGroup.AddPermission(BIMPlatformPermissions.Project.Manage, L("Permission:Project"), MultiTenancySides.Tenant);
            projectPermissions.AddChild(BIMPlatformPermissions.Project.Create, L("Permission:Create"), MultiTenancySides.Tenant);
            projectPermissions.AddChild(BIMPlatformPermissions.Project.Update, L("Permission:Update"), MultiTenancySides.Tenant);
            projectPermissions.AddChild(BIMPlatformPermissions.Project.Delete, L("Permission:Delete"), MultiTenancySides.Tenant);
            projectPermissions.AddChild(BIMPlatformPermissions.Project.Default, L("Permission:Default"), MultiTenancySides.Tenant);
            #endregion

            #region document 
            var documentGroup = context.AddGroup(BIMPlatformPermissionGroupDefinition.DocumentGroup, L("Permission:Document"), MultiTenancySides.Tenant);
            var documentPermissions = documentGroup.AddPermission(BIMPlatformPermissions.Document.Manage, L("Permission:Document"), MultiTenancySides.Tenant);
            documentPermissions.AddChild(BIMPlatformPermissions.Document.Create, L("Permission:Create"), MultiTenancySides.Tenant);
            documentPermissions.AddChild(BIMPlatformPermissions.Document.Update, L("Permission:Update"), MultiTenancySides.Tenant);
            documentPermissions.AddChild(BIMPlatformPermissions.Document.Delete, L("Permission:Delete"), MultiTenancySides.Tenant);
            documentPermissions.AddChild(BIMPlatformPermissions.Document.Default, L("Permission:Default"), MultiTenancySides.Tenant);
            #endregion

            #region 工单
            var constructionSupervisionGroup = context.AddGroup(BIMPlatformPermissionGroupDefinition.ConstructionSupervision, L("Permission:SafetyIssue"), MultiTenancySides.Tenant);
            var safetyIssuePermissions = constructionSupervisionGroup.AddPermission(BIMPlatformPermissions.SafetyIssue.Manage, L("Permission:SafetyIssue"), MultiTenancySides.Tenant);
            safetyIssuePermissions.AddChild(BIMPlatformPermissions.SafetyIssue.Create, L("Permission:Create"), MultiTenancySides.Tenant);
            safetyIssuePermissions.AddChild(BIMPlatformPermissions.SafetyIssue.Update, L("Permission:Update"), MultiTenancySides.Tenant);
            safetyIssuePermissions.AddChild(BIMPlatformPermissions.SafetyIssue.Delete, L("Permission:Delete"), MultiTenancySides.Tenant);
            safetyIssuePermissions.AddChild(BIMPlatformPermissions.SafetyIssue.Default, L("Permission:Default"), MultiTenancySides.Tenant);

            var qualityIssuePermissions = constructionSupervisionGroup.AddPermission(BIMPlatformPermissions.QualityIssue.Manage, L("Permission:QualityIssue"), MultiTenancySides.Tenant);
            qualityIssuePermissions.AddChild(BIMPlatformPermissions.QualityIssue.Create, L("Permission:Create"), MultiTenancySides.Tenant);
            qualityIssuePermissions.AddChild(BIMPlatformPermissions.QualityIssue.Update, L("Permission:Update"), MultiTenancySides.Tenant);
            qualityIssuePermissions.AddChild(BIMPlatformPermissions.QualityIssue.Delete, L("Permission:Delete"), MultiTenancySides.Tenant);
            qualityIssuePermissions.AddChild(BIMPlatformPermissions.QualityIssue.Default, L("Permission:Default"), MultiTenancySides.Tenant);

            var supervisorLogPermissions = constructionSupervisionGroup.AddPermission(BIMPlatformPermissions.SupervisorLog.Manage, L("Permission:SupervisorLog"), MultiTenancySides.Tenant);
            supervisorLogPermissions.AddChild(BIMPlatformPermissions.SupervisorLog.Create, L("Permission:Create"), MultiTenancySides.Tenant);
            supervisorLogPermissions.AddChild(BIMPlatformPermissions.SupervisorLog.Update, L("Permission:Update"), MultiTenancySides.Tenant);
            supervisorLogPermissions.AddChild(BIMPlatformPermissions.SupervisorLog.Delete, L("Permission:Delete"), MultiTenancySides.Tenant);
            supervisorLogPermissions.AddChild(BIMPlatformPermissions.SupervisorLog.Default, L("Permission:Default"), MultiTenancySides.Tenant);
            #endregion

            #region 劳务
            var labourServiceGroup = context.AddGroup(BIMPlatformPermissionGroupDefinition.LabourServiceGroup, L("Permission:Attendance"), MultiTenancySides.Tenant);
            var labourServicePermissions = labourServiceGroup.AddPermission(BIMPlatformPermissions.Attendance.Manage, L("Permission:Attendance"), MultiTenancySides.Tenant);
            labourServicePermissions.AddChild(BIMPlatformPermissions.Attendance.ViewLeave, L("Permission:Create"), MultiTenancySides.Tenant);
            labourServicePermissions.AddChild(BIMPlatformPermissions.Attendance.ApproveLeave, L("Permission:Update"), MultiTenancySides.Tenant);
            labourServicePermissions.AddChild(BIMPlatformPermissions.Attendance.ViewSheWolf, L("Permission:Delete"), MultiTenancySides.Tenant);
            labourServicePermissions.AddChild(BIMPlatformPermissions.Attendance.ManageSheWolf, L("Permission:Delete"), MultiTenancySides.Tenant);
            labourServicePermissions.AddChild(BIMPlatformPermissions.Attendance.RealTimeStatistics, L("Permission:Delete"), MultiTenancySides.Tenant);
            labourServicePermissions.AddChild(BIMPlatformPermissions.Attendance.WeekAndMounthStatistics, L("Permission:Delete"), MultiTenancySides.Tenant);
            labourServicePermissions.AddChild(BIMPlatformPermissions.Attendance.Default, L("Permission:Default"), MultiTenancySides.Tenant);

            #endregion

            #region 生产力
            var forcesOfProductionGroup = context.AddGroup(BIMPlatformPermissionGroupDefinition.ForcesOfProductionGroup, L("Permission:EfficiencyAnalysis"), MultiTenancySides.Tenant);
            var forcesOfProductionPermissions = forcesOfProductionGroup.AddPermission(BIMPlatformPermissions.EfficiencyAnalysis.Manage, L("Permission:EfficiencyAnalysis"), MultiTenancySides.Tenant);
            forcesOfProductionPermissions.AddChild(BIMPlatformPermissions.EfficiencyAnalysis.Default, L("Permission:Default"), MultiTenancySides.Tenant);

            #endregion


            #region 设备
            var capitalItemsGroup = context.AddGroup(BIMPlatformPermissionGroupDefinition.CapitalItemsGroup, L("Permission:Equipment"), MultiTenancySides.Tenant);
            var capitalItemsPermissions = capitalItemsGroup.AddPermission(BIMPlatformPermissions.Equipment.Manage, L("Permission:Equipment"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ViewTowerCrane, L("Permission:Create"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ViewEnvironment, L("Permission:Update"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ViewVideo, L("Permission:Delete"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ManageVideo, L("Permission:Default"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ViewElevator, L("Permission:Create"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ViewCar, L("Permission:Update"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ViewHighModulus, L("Permission:Delete"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ViewDeepExcavation, L("Permission:Default"), MultiTenancySides.Tenant);
            capitalItemsPermissions.AddChild(BIMPlatformPermissions.Equipment.ViewLoadometer, L("Permission:Default"), MultiTenancySides.Tenant);

            #endregion

            #region 知识库
            var knowledgeBaseGroup = context.AddGroup(BIMPlatformPermissionGroupDefinition.KnowledgeBaseGroup, L("Permission:KnowledgeBase"), MultiTenancySides.Tenant);
            var knowledgeBasePermissions = knowledgeBaseGroup.AddPermission(BIMPlatformPermissions.KnowledgeBase.Manage, L("Permission:KnowledgeBase"), MultiTenancySides.Tenant);
            knowledgeBasePermissions.AddChild(BIMPlatformPermissions.KnowledgeBase.ViewQA, L("Permission:Create"), MultiTenancySides.Tenant);
            knowledgeBasePermissions.AddChild(BIMPlatformPermissions.KnowledgeBase.ViewRegulation, L("Permission:Update"), MultiTenancySides.Tenant);
           
            #endregion
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BIMPlatformResource>(name);
        }
         
    }

    public static class BIMPlatformPermissionGroupDefinition
    {
        /// <summary>
        /// 项目管理相关权限
        /// </summary>
        public const string ProjectGroup = "ProjectManagement";
        /// <summary>
        /// 文档管理
        /// </summary>
        public const string DocumentGroup = "DocumentManagement";
        /// <summary>
        /// 工单
        /// </summary>
        public const string ConstructionSupervision = "ConstructionSupervisionManagement";
        /// <summary>
        /// 组织部门管理
        /// </summary>
        public const string OrganizationGroup = "OrganizationManagement";
        /// <summary>
        /// 劳务管理
        /// </summary>
        public const string LabourServiceGroup = "LabourServiceManagement";
        /// <summary>
        /// 生产力
        /// </summary>
        public const string ForcesOfProductionGroup = "ForcesOfProductionManagement";
        /// <summary>
        /// 设备
        /// </summary>
        public const string CapitalItemsGroup = "CapitalItemsManagement";
        /// <summary>
        /// 知识库
        /// </summary>
        public const string KnowledgeBaseGroup = "KnowledgeBase";
    }
}
