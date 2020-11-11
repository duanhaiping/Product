using BIMPlatform.Application.Contracts.ProjectDto;
using BIMPlatform.Infrastructure.Project.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace BIMPlatform.Module.Project.Services.Default
{

    public class ProjectConfigurationService : BaseService, IProjectConfigurationService
    {
        public ProjectConfigurationService(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        public IList<IDictionary<string, object>> GetDesignChangeCategories(string modeName = "", string name = "")
        {
            List<IDictionary<string, object>> dics = new List<IDictionary<string, object>>();
            string fileName = "PickListConfiguration.xml";
            string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "SystemConfiguration");
            string groupSettingsFile = Path.Combine(folderPath, fileName);
            if (string.IsNullOrEmpty(modeName))
            {
                modeName = "PickList";
            }
            if (File.Exists(groupSettingsFile))
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(groupSettingsFile);
                    foreach (XmlNode primaryElement in xmlDoc.SelectNodes("//" + modeName))
                    {
                        if (primaryElement is XmlComment)
                            continue;

                        IDictionary<string, object> primary = new Dictionary<string, object>();
                        string primaryCategory = primaryElement.Attributes["SystemName"].Value;
                        string primaryName = primaryElement.Attributes["Name"].Value;
                        if (string.IsNullOrEmpty(name))
                        {
                            primary.Add("SystemName", primaryCategory);
                            primary.Add("Name", primaryName);
                            primary.Add("Secondaries", ListNodes(primaryElement));
                            dics.Add(primary);
                            //ListNodes(dics, primaryElement, primary, primaryCategory, primaryName);
                        }
                        else if (name.Trim() == primaryName.Trim() || name.Trim() == primaryCategory.Trim())
                        {
                            primary.Add("SystemName", primaryCategory);
                            primary.Add("Name", primaryName);
                            primary.Add("Secondaries", ListNodes(primaryElement));
                            dics.Add(primary);
                        }

                    }
                }
                catch (Exception ex)
                {
                    string error = L["ProjectState_LoadSettingError"];
                    // ServerLogger.Error(error, ex);
                    throw new Exception(error);
                }
            }
            else
            {
                throw new Exception(string.Format(L["Group_MissingSettingFile"], fileName));
            }

            return dics;
        }

        public virtual IList<EntityNamingRule> GetProjectAbb(string projectName, string ruleName = "")
        {
            string fileName = "ProjectConfiguration.xml";
            string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "SystemConfiguration");
            string groupSettingsFile = Path.Combine(folderPath, fileName);
            string proAbb = string.Empty;
            List<EntityNamingRule> lists = new List<EntityNamingRule>();
            if (File.Exists(groupSettingsFile))
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(groupSettingsFile);
                    XmlNodeList node = xmlDoc.SelectNodes("//Project");
                    foreach (XmlNode primaryElement in node)
                    {
                        if (primaryElement is XmlComment)
                            continue;
                        if (primaryElement.Attributes["Name"].Value == projectName)
                        {

                            if (!string.IsNullOrEmpty(ruleName))
                            {
                                foreach (XmlNode rule in primaryElement.ChildNodes)
                                {
                                    if (ruleName == rule.Attributes["Name"].Value)
                                    {
                                        EntityNamingRule namingRule = new EntityNamingRule();
                                        namingRule.RuleName = ruleName;
                                        namingRule.RuleValue = rule.InnerText;
                                        lists.Add(namingRule);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (XmlNode rule in primaryElement.ChildNodes)
                                {
                                    EntityNamingRule namingRule = new EntityNamingRule();
                                    namingRule.RuleName = ruleName;
                                    namingRule.RuleValue = rule.InnerText;
                                    lists.Add(namingRule);
                                }
                            }
                            break;
                        }
                    }
                    return lists;
                }
                catch (Exception ex)
                {
                    string error =L["ProjectState_LoadSettingError"];
                    // ServerLogger.Error(error, ex);
                    throw new Exception(error);
                }
            }
            return lists;

        }
        #region 问题获取分类
        public IDictionary<string, IDictionary<string, object>> GetXMLIssueCategory()
        {
            IDictionary<string, IDictionary<string, object>> dics = new Dictionary<string, IDictionary<string, object>>();
            string fileName = "IssueCategory.xml";
            string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "SystemConfiguration");
            string groupSettingsFile = Path.Combine(folderPath, fileName);
            if (File.Exists(groupSettingsFile))
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(groupSettingsFile);
                    foreach (XmlNode primaryElement in xmlDoc.SelectNodes("//IssueCategoryParent"))
                    {
                        if (primaryElement is XmlComment)
                            continue;

                        IDictionary<string, object> primary = new Dictionary<string, object>();
                        string primaryName = primaryElement.Attributes["Name"].Value;
                        StringBuilder builder = new StringBuilder();
                        bool isbegin = true;
                        GetLeafCategory(primaryElement, primary, builder, primaryName, isbegin);
                        if (primary.Count > 1)
                        {
                            dics.Add(primaryName, primary);
                        }
                        else
                        {
                            dics.Add(primaryName, new Dictionary<string, object>());
                        }

                    }
                }
                catch (Exception ex)
                {
                    string error = L["ProjectState_LoadSettingError"];
                    // ServerLogger.Error(error, ex);
                    throw new Exception(error);
                }
            }
            else
            {
                throw new Exception(string.Format(L["Group_MissingSettingFile"], fileName));
            }

            return dics;
        }

        private void GetLeafCategory(XmlNode cat, IDictionary<string, object> dic, StringBuilder builder, string parentName, bool isbegin)
        {
            if (!isbegin)
            {
                builder.Append(cat.Attributes["Name"].Value);
                builder.Append("/");
            }
            else
            {
                isbegin = false;
            }
            int i = 0;
            int length = cat.Attributes["Name"].Value.Length + 1;

            foreach (XmlNode child in cat.ChildNodes)
            {
                i++;
                GetLeafCategory(child, dic, builder, parentName, isbegin);
                if (child.ChildNodes.Count == 0)
                {
                    builder.Remove((builder.Length - child.Attributes["Name"].Value.Length), child.Attributes["Name"].Value.Length);
                }
                if (i == cat.ChildNodes.Count)
                {
                    i = 0;
                    if (builder.Length > 1)
                    {
                        builder.Remove((builder.Length - length), length);

                    }
                }

            }

            if (cat.ChildNodes.Count == 0)
            {
                string keyn = cat.Attributes["Name"].Value;
                string builders = builder.ToString();
                builders = builders.Replace(keyn + "/", "");
                string[] arr = builders.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Count() > 0)
                {
                    keyn = arr.LastOrDefault() + "/" + keyn;
                }
                string name = "";
                if (string.IsNullOrEmpty(builder.ToString()))
                {
                    name = cat.Attributes["Name"].Value;
                }
                else
                {
                    name = builder.Remove((builder.Length - 1), 1).ToString();
                }
                dic.Add(keyn, name);


            }

        }
        #endregion

        private static void ListNodes(List<IDictionary<string, object>> dics, XmlNode primaryElement, IDictionary<string, object> primary, string primaryCategory, string primaryName)
        {
            List<Dictionary<string, object>> Secondaries = new List<Dictionary<string, object>>();

            foreach (XmlNode secondaryElement in primaryElement.ChildNodes)
            {
                Dictionary<string, object> second = new Dictionary<string, object>();
                if (secondaryElement is XmlComment)
                    continue;
                string value = secondaryElement.Attributes["Value"].Value;
                string name = secondaryElement.Attributes["Name"].Value;
                string description = secondaryElement.Attributes["Description"].Value;
                second.Add("Value", value);
                second.Add("Name", name);
                second.Add("Description", description);
                second.Add("Secondaries", description);
                Secondaries.Add(second);
            }
            primary.Add("SystemName", primaryCategory);
            primary.Add("Name", primaryName);
            primary.Add("Secondaries", Secondaries);
            dics.Add(primary);
        }

        private static List<Dictionary<string, object>> ListNodes( XmlNode primaryElement)
        {
            List<Dictionary<string, object>> Secondaries = new List<Dictionary<string, object>>();

            foreach (XmlNode secondaryElement in primaryElement.ChildNodes)
            {
                Dictionary<string, object> second = new Dictionary<string, object>();
                if (secondaryElement is XmlComment)
                    continue;
                string value = secondaryElement.Attributes["Value"].Value;
                string name = secondaryElement.Attributes["Name"].Value;
                string description = secondaryElement.Attributes["Description"].Value;
                second.Add("Value", value);
                second.Add("Name", name);
                second.Add("Description", description);
                second.Add("Secondaries", ListNodes(secondaryElement));
                Secondaries.Add(second);
            }
            return Secondaries;
    }
    }
}
