using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using XMLDO;

namespace ConfigManager
{
    public class ConfigCore
    {
        #region Singleton    
        public static ConfigCore Instance { get { return SafeConfigCore.instance; } }

        private class SafeConfigCore
        {
            static SafeConfigCore() { }
            internal static readonly ConfigCore instance = new ConfigCore();
        }
        #endregion

        private string ConfigFolderPath;

        private XmlDocument XmlDoc;

        public void ExportXML(ConfigData data)
        {
            if (data == null) return;

            XmlDoc = new XmlDocument();
            try
            {
                XmlNode rootNode = XmlDoc.CreateElement(data.RootNode.NodeName == "" ? "NewNode" : data.RootNode.NodeName);
                XmlDoc.AppendChild(rootNode);

                if (data.RootNode.SubNodes != null)
                {
                    for (int k = 0; k < data.RootNode.SubNodes.Length; k++)
                    {
                        AddSubNodes(data.RootNode.SubNodes[k], rootNode);
                    }
                }
                ExportXml_Rec(XmlDoc, data.ConfigName, data.ParentFolder);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        //Recursive Method
        private void ExportXml_Rec(XmlDocument doc, string name, string parentFolder = "")
        {
            if (doc == null) return;

#if UNITY_EDITOR
            ConfigFolderPath = Application.dataPath + "/../Configs";
#else
            ConfigFolderPath = Application.dataPath + "/../Configs";
#endif

            string currentPath = ConfigFolderPath;
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
            if (parentFolder != "")
            {
                currentPath += "/" + parentFolder;
                if (!Directory.Exists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);
                }
            }
            doc.Save(currentPath + "/" + name + ".xml");
        }

        private void AddSubNodes(Node subNode, XmlNode parentNode)
        {
            if (parentNode == null) return;

            XmlNode rootNode = XmlDoc.CreateElement(subNode.NodeName);
            parentNode.AppendChild(rootNode);
            if (subNode.Attributes != null)
                AddAttribute(subNode.Attributes, rootNode);
            if (subNode.SubNodes != null)
            {
                for (int i = 0; i < subNode.SubNodes.Length; i++)
                {
                    AddSubNodes(subNode.SubNodes[i], rootNode);
                }
            }
        }
        private void AddAttribute(Attribute[] attributes, XmlNode parentNode)
        {
            if (attributes.Length == 0) return;

            for (int k = 0; k < attributes.Length; k++)
            {
                XmlAttribute attribute = XmlDoc.CreateAttribute(attributes[k].AttributeName == "" ? "NewAttribute" : attributes[k].AttributeName);
                attribute.Value = attributes[k].Value;
                parentNode.Attributes.Append(attribute);
            }
        }
    }
}