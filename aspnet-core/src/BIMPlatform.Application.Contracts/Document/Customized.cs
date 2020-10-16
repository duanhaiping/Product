using System;
using System.Collections.Generic;
using System.Xml;

namespace BIMPlatform.Application.Contracts.DocumentDataInfo
{
    public class XmlOption
    {
        public static string GetXmlProperty(List<Customized> property)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement root = xmlDoc.CreateElement("Preferences");
                xmlDoc.AppendChild(root);

                foreach (var item in property)
                {
                    XmlElement xmlFolder = xmlDoc.CreateElement("Property");
                    xmlFolder.SetAttribute("PropertyName", item.PropertyName);
                    //xmlFolder.SetAttribute("Type", item.Type);
                    xmlFolder.SetAttribute("Value", item.Value);
                    //xmlFolder.SetAttribute("DisplayName", item.DisplayName);
                    root.AppendChild(xmlFolder);
                }

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Customized> LoadPropertys(string pr)
        {
            List<Customized> list = new List<Customized>();
            if (!string.IsNullOrEmpty(pr))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(pr);

                XmlNode localPropertys = xmlDoc.SelectSingleNode("//Preferences");
                foreach (XmlNode item in localPropertys)
                {
                    list.Add(new Customized(item));
                }
            }
            return list;
        }

        public static bool CheckXmlProperty(string pr, Customized property)
        {
            if (!string.IsNullOrEmpty(pr))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(pr);

                XmlNode localPropertys = xmlDoc.SelectSingleNode("//Preferences");
                foreach (XmlNode item in localPropertys)
                {
                    if (item.Attributes["PropertyName"].Value == property.PropertyName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class Customized
    {
        public Customized()
        {
        }
        public Customized(XmlNode node)
        {
            PropertyName = node.Attributes["PropertyName"].Value;
            //Type = node.Attributes["Type"].Value;
            Value = node.Attributes["Value"].Value;
            //DisplayName = node.Attributes["DisplayName"].Value;
        }

        public string PropertyName { get; set; }
        //public string Type { get; set; }
        public string Value { get; set; }
        //public string DisplayName { get; set; }
    }
}
