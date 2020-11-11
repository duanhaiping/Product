using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BIMPlatform.ToolKits.Helper
{
    public class XMLHelper
    {
        //project
        private const string Projects = "Projects";
        private const string Project = "Project";
        private const string Id = "Id";
        private const string LastSelected = "LastSelected";
        private const string ElementIDs = "ElementIDs";
        private const string ElementID = "ElementID";


        //common
        private const string AllDocument = "AllDocument";
        private const string Tags = "Tags";
        private const string Tag = "Tag";
        private const string TName = "TName";
        private const string TCreateTime = "TCreateTime";

        //System
        public const string KnowledgeItmeKeywords = "KnowledgeItmeKeywords";
        public const string SystemBulletinKeywords = "SystemBulletinKeywords";


        private static string GetSelectSingleNode(string node)
        {
            return "//" + node;
        }


        public static XmlDocument UpdateUserProjectPreference(string projectId, string preference)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                if (string.IsNullOrEmpty(preference))
                {
                    XmlElement rootElement = document.CreateElement(Projects);
                    XmlElement nodeElement = document.CreateElement(Project);
                    nodeElement.SetAttribute(Id, projectId);
                    nodeElement.SetAttribute(LastSelected, true.ToString());
                    rootElement.AppendChild(nodeElement);
                    document.AppendChild(rootElement);
                }
                else
                {
                    document.LoadXml(preference);
                    XmlNode lobjRootNode = document.SelectSingleNode("//Projects");
                    if (lobjRootNode == null)
                    {
                        lobjRootNode = document.CreateElement(Projects);
                        document.AppendChild(lobjRootNode);
                    }

                    XmlNodeList lcolProjectsNodes = document.SelectNodes("//Projects/Project");
                    XmlElement matchedProjectNode = null;
                    foreach (XmlElement projectNode in lcolProjectsNodes)
                    {
                        if (projectNode.Attributes[Id].Value != projectId)
                        {
                            projectNode.SetAttribute(LastSelected, false.ToString());
                        }
                        else
                        {
                            matchedProjectNode = projectNode as XmlElement;
                            projectNode.SetAttribute(LastSelected, true.ToString());
                        }
                    }

                    if (matchedProjectNode == null)
                    {
                        matchedProjectNode = document.CreateElement(Project);
                        matchedProjectNode.SetAttribute(Id, projectId);
                        matchedProjectNode.SetAttribute(LastSelected, true.ToString());
                        lobjRootNode.AppendChild(matchedProjectNode);
                    }
                }

                return document;
            }
            catch
            {
                return null;
            }
        }


        public static int GetLastSelectedProjectByUserPreference(string preference)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(preference);
                int projectId = 0;
                XmlNodeList lcolProjectsNodes = xmlDoc.SelectNodes("//Projects/Project");
                foreach (XmlNode projNode in lcolProjectsNodes)
                {
                    if (projNode.Attributes[LastSelected] == null)
                        continue;

                    bool lastSelected = bool.Parse(projNode.Attributes[LastSelected].Value);
                    if (lastSelected)
                    {
                        projectId = int.Parse(projNode.Attributes[Id].Value);
                        break;
                    }
                }
                return projectId;
            }
            catch (Exception ex)
            {
               
                return 0;
            }
        }

        public static XmlDocument CreateXmlTaskByElements(string element)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                if (string.IsNullOrEmpty(element))
                {
                    return document;
                }
                else
                {
                    element = element.Replace(",", "</EID><EID>");
                    element = "<ElementIDs><EID>" + element + "</EID></ElementIDs>";
                    document.LoadXml(element);
                }
                return document;
            }
            catch
            {
                return null;
            }
        }

        public static XmlDocument UpdateUserTags(IEnumerable<string> tags, string projectId, string preference)
        {
            XmlDocument document = new XmlDocument();
            if (string.IsNullOrEmpty(preference))
            {
                XmlElement rootElement = document.CreateElement(Projects);
                XmlElement nodeElement = document.CreateElement(Project);
                nodeElement.SetAttribute(Id, projectId);
                nodeElement.SetAttribute(LastSelected, true.ToString());
                rootElement.AppendChild(nodeElement);
                document.AppendChild(rootElement);
            }
            else
            {
                document.LoadXml(preference);
            }

            XmlNodeList lcolProjectsNodes = document.SelectNodes("//Projects/Project");
            foreach (XmlNode node in lcolProjectsNodes)
            {
                if (node.Attributes[Id].Value == projectId)
                {
                    XmlNode tagsNodes = node.SelectSingleNode("//Tags");
                    if (tagsNodes == null)
                    {
                        XmlElement NewnodeElement = document.CreateElement(Tags);
                        node.AppendChild(NewnodeElement);
                        tagsNodes = node.SelectSingleNode("//Tags");
                    }

                    if (tags != null)
                    {
                        foreach (var tag in tags)
                        {
                            var exists = false;
                            foreach (XmlNode tagNode in tagsNodes.ChildNodes)
                            {
                                if (tagNode.Attributes[TName].Value == tag)
                                {
                                    tagNode.Attributes[TCreateTime].Value = DateTime.Now.ToString();
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                XmlElement newTagNodeElement = document.CreateElement(Tag);
                                newTagNodeElement.SetAttribute(TName, tag);
                                newTagNodeElement.SetAttribute(TCreateTime, DateTime.Now.ToString());
                                tagsNodes.AppendChild(newTagNodeElement);
                            }
                        }
                    }
                }
            }
            return document;
        }

        public static XmlDocument UpdateRecentUsers(IEnumerable<int> userIDs, string projectId, string preference)
        {
            XmlDocument document = new XmlDocument();
            if (string.IsNullOrEmpty(preference))
            {
                XmlElement rootElement = document.CreateElement(Projects);
                XmlElement nodeElement = document.CreateElement(Project);
                nodeElement.SetAttribute(Id, projectId);
                nodeElement.SetAttribute(LastSelected, true.ToString());
                rootElement.AppendChild(nodeElement);
                document.AppendChild(rootElement);
            }
            else
            {
                document.LoadXml(preference);
            }

            XmlNodeList lcolProjectsNodes = document.SelectNodes("//Projects/Project");
            foreach (XmlNode node in lcolProjectsNodes)
            {
                if (node.Attributes[Id].Value == projectId)
                {
                    XmlNode recentUsersNodes = node.SelectSingleNode("//RecentUsers");
                    if (recentUsersNodes == null)
                    {
                        XmlElement NewnodeElement = document.CreateElement("RecentUsers");
                        node.AppendChild(NewnodeElement);
                        recentUsersNodes = node.SelectSingleNode("//RecentUsers");
                    }

                    if (userIDs != null)
                    {
                        foreach (var userID in userIDs)
                        {
                            var exists = false;
                            foreach (XmlNode userNode in recentUsersNodes.ChildNodes)
                            {
                                if (userNode.Attributes["ID"].Value == userID.ToString())
                                {
                                    userNode.Attributes[TCreateTime].Value = DateTime.Now.ToString();
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                XmlElement newUserNodeElement = document.CreateElement(Tag);
                                newUserNodeElement.SetAttribute("ID", userID.ToString());
                                newUserNodeElement.SetAttribute(TCreateTime, DateTime.Now.ToString());
                                recentUsersNodes.AppendChild(newUserNodeElement);
                            }
                        }
                    }
                }
            }
            return document;
        }

        public static XmlDocument UpdateRecentMessagedUsers(IEnumerable<int> userIDs, string projectId, string preference)
        {
            XmlDocument document = new XmlDocument();
            if (string.IsNullOrEmpty(preference))
            {
                XmlElement rootElement = document.CreateElement(Projects);
                XmlElement nodeElement = document.CreateElement(Project);
                nodeElement.SetAttribute(Id, projectId);
                nodeElement.SetAttribute(LastSelected, true.ToString());
                rootElement.AppendChild(nodeElement);
                document.AppendChild(rootElement);
            }
            else
            {
                document.LoadXml(preference);
            }

            XmlNodeList lcolProjectNodes = document.SelectNodes("//Projects/Project");
            foreach (XmlNode node in lcolProjectNodes)
            {
                if (node.Attributes[Id].Value == projectId)
                {
                    XmlNode recentMessagedUsersNodes = node.SelectSingleNode("//RecentMessagedUsers");
                    if (recentMessagedUsersNodes == null)
                    {
                        XmlElement newNodeElement = document.CreateElement("RecentMessagedUsers");
                        node.AppendChild(newNodeElement);
                        recentMessagedUsersNodes = node.SelectSingleNode("//RecentMessagedUsers");
                    }

                    if (userIDs != null)
                    {
                        foreach (var userID in userIDs)
                        {
                            var exists = false;
                            foreach (XmlNode userNode in recentMessagedUsersNodes.ChildNodes)
                            {
                                if (userNode.Attributes["ID"].Value == userID.ToString())
                                {
                                    userNode.Attributes[TCreateTime].Value = DateTime.Now.ToString();
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                XmlElement newUserNodeElement = document.CreateElement(Tag);
                                newUserNodeElement.SetAttribute("ID", userID.ToString());
                                newUserNodeElement.SetAttribute(TCreateTime, DateTime.Now.ToString());
                                recentMessagedUsersNodes.AppendChild(newUserNodeElement);
                            }
                        }
                    }
                }
            }
            return document;
        }

        public static XmlDocument UpdateSystemDataKeywords(IEnumerable<string> keywords, string preference, string keywordType)
        {
            XmlDocument document = new XmlDocument();
            if (string.IsNullOrEmpty(preference))
            {
                XmlElement rootElement = document.CreateElement(Projects);
                document.AppendChild(rootElement);
            }
            else
            {
                document.LoadXml(preference);
            }

            XmlNode lobjProjectsNode = document.SelectSingleNode(GetSelectSingleNode(Projects));
            XmlNode keywordsNode = lobjProjectsNode.SelectSingleNode(GetSelectSingleNode(keywordType));
            if (keywordsNode == null)
            {
                XmlElement newNodeElement = document.CreateElement(keywordType);
                lobjProjectsNode.AppendChild(newNodeElement);
                keywordsNode = lobjProjectsNode.SelectSingleNode(GetSelectSingleNode(keywordType));
            }

            if (keywords != null)
            {
                foreach (var tag in keywords)
                {
                    var exists = false;
                    foreach (XmlNode keywordNode in keywordsNode)
                    {
                        if (keywordNode.Attributes[TName].Value == tag)
                        {
                            keywordNode.Attributes[TCreateTime].Value = DateTime.Now.ToString();
                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        XmlElement newKeywordNodeElement = document.CreateElement(Tag);
                        newKeywordNodeElement.SetAttribute(TName, tag);
                        newKeywordNodeElement.SetAttribute(TCreateTime, DateTime.Now.ToString());
                        keywordsNode.AppendChild(newKeywordNodeElement);
                    }
                }
            }
            return document;
        }

        public static string GetSystemDataKeywords(string preference, string keywordType)
        {
            List<object> tags = new List<object>();
            XmlDocument existdocument = new XmlDocument();
            existdocument.LoadXml(preference);
            XmlNode lobjProjectsNode = existdocument.SelectSingleNode(GetSelectSingleNode(Projects));
            if (lobjProjectsNode != null)
            {
                XmlNode tagsNodes = lobjProjectsNode.SelectSingleNode(GetSelectSingleNode(keywordType));
                if (tagsNodes != null)
                {
                    foreach (XmlNode tagnode in tagsNodes)
                    {
                        tags.Add(new { Name = tagnode.Attributes[TName].Value, CreateTime = Convert.ToDateTime(tagnode.Attributes[TCreateTime].Value) });
                    }
                }
            }
            string strtags = NewtonJson.SerializeObject(tags);
            return strtags;
        }

        public static string GetUserTagXML(string projectId, string preference)
        {
            List<object> tags = new List<object>();
            XmlDocument existdocument = new XmlDocument();
            existdocument.LoadXml(preference);
            XmlNodeList lcolProjectNodes = existdocument.SelectNodes("//Projects/Project");
            if (lcolProjectNodes != null)
            {
                foreach (XmlNode node in lcolProjectNodes)
                {
                    if (node.Attributes[Id].Value == projectId)
                    {
                        XmlNode tagsNodes = node.SelectSingleNode("//Tags");
                        if (tagsNodes != null)
                        {
                            foreach (XmlNode tagnode in tagsNodes)
                            {
                                tags.Add(new { Name = tagnode.Attributes[TName].Value, CreateTime = Convert.ToDateTime(tagnode.Attributes[TCreateTime].Value) });
                            }
                        }
                    }
                }
            }
            string strtags = NewtonJson.SerializeObject(tags);
            return strtags;
        }

        public static string GetRecentUsersXML(string projectId, string preference)
        {
            List<object> tags = new List<object>();
            XmlDocument existdocument = new XmlDocument();
            existdocument.LoadXml(preference);
            XmlNodeList lcolProjectNodes = existdocument.SelectNodes("//Projects/Project");
            foreach (XmlNode node in lcolProjectNodes)
            {
                if (node.Attributes[Id].Value == projectId)
                {
                    XmlNode recentUsersNodes = node.SelectSingleNode("//RecentUsers");
                    if (recentUsersNodes != null)
                    {
                        foreach (XmlNode userNode in recentUsersNodes.ChildNodes)
                        {
                            tags.Add(new { ID = userNode.Attributes["ID"].Value, CreateTime = Convert.ToDateTime(userNode.Attributes[TCreateTime].Value) });
                        }
                    }
                }
            }
            string strtags = NewtonJson.SerializeObject(tags);
            return strtags;
        }

        public static string GetRecentMessagedUsersXML(string projectId, string preference)
        {
            List<object> users = new List<object>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(preference);
            XmlNodeList lcolProjectNodes = xmlDocument.SelectNodes("//Projects/Project");
            foreach (XmlNode node in lcolProjectNodes)
            {
                if (node.Attributes[Id].Value == projectId)
                {
                    XmlNode recentUsersNodes = node.SelectSingleNode("//RecentMessagedUsers");
                    if (recentUsersNodes != null)
                    {
                        foreach (XmlNode userNode in recentUsersNodes.ChildNodes)
                        {
                            users.Add(new
                            {
                                ID = userNode.Attributes["ID"].Value,
                                CreateTime = Convert.ToDateTime(userNode.Attributes[TCreateTime].Value)
                            });
                        }
                    }
                }
            }
            string strUsers = NewtonJson.SerializeObject(users);
            return strUsers;
        }

        public static string GetRequestXml(string url, string method, Dictionary<string, object> obj)
        {
            string request = string.Empty;

            XmlDocument requestDoc = new XmlDocument();
            XmlNode root = requestDoc.CreateElement("Request");
            requestDoc.AppendChild(root);

            XmlNode urlnode = requestDoc.CreateElement("PlatformURL");
            urlnode.InnerText = url;
            root.AppendChild(urlnode);

            XmlNode methodnode = requestDoc.CreateElement("Method");
            methodnode.InnerText = method;
            root.AppendChild(methodnode);

            XmlNode parametersnode = requestDoc.CreateElement("Parameters");

            foreach (var item in obj)
            {
                XmlNode par = requestDoc.CreateElement(item.Key);
                par.InnerText = item.Value == null ? "" : item.Value.ToString();
                parametersnode.AppendChild(par);
            }
            root.AppendChild(parametersnode);

            return requestDoc.InnerXml;
        }

    }
}
