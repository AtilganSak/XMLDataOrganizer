using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XMLDO;

namespace ConfigManager
{
    public class ConfigIO : MonoBehaviour
    {
        #region Singleton    
        private static ConfigIO instance;

        public static ConfigIO Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject configManager = GameObject.Find("ConfigManager");
                    if (configManager == null)
                    {
                        configManager = new GameObject("ConfigManager");
                        instance = configManager.AddComponent<ConfigIO>();
                    }
                    else
                    {
                        instance = configManager.GetComponent<ConfigIO>();
                        if (instance == null)
                            instance = configManager.AddComponent<ConfigIO>();
                    }
                }
                return instance;
            }
        }
        #endregion

        public delegate void SavedDataDelegate();
        public event SavedDataDelegate SavedDataEvent;

        public List<ConfigData> configDatas;

        private int index = 1;
        private string[] splitPath;

        public void SaveData(string configName, RowHandler root)
        {
            if (root == null) return;
            ConfigData data = configDatas.Find(x => x.ConfigName == configName);

            Save_Rec(data.RootNode, root);

#if UNITY_EDITOR
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
#endif
            if (SavedDataEvent != null)
                SavedDataEvent.Invoke();
        }

        //Recursive Method
        private void Save_Rec(Node node, RowHandler subRow)
        {
            if (subRow.attributes != null)
            {
                for (int i = 0; i < subRow.attributes.Count; i++)
                {
                    node.Attributes[i].Value = subRow.attributes[i].GetValue();
                }
            }
            for (int i = 0; i < subRow.subNodes.Count; i++)
            {
                Save_Rec(node.SubNodes[i], subRow.subNodes[i]);
            }
        }

        public List<ConfigData> GetConfigDatas() => configDatas;

        /// <summary>
        /// CaseSensivity = Consider the upper and lower case in the given path?
        /// </summary>
        /// <param name="path"></param>
        /// <param name="CaseSensivity"></param>
        /// <returns></returns>
        public string GetParameter(string path, bool CaseSensivity = true)
        {
            if (path == "") return "";

            string result = "";

            splitPath = path.Split('/');
            if (splitPath != null)
            {
                if (splitPath.Length > 1)
                {
                    string configName = splitPath[0];

                    ConfigData data = configDatas.Find(x => (!CaseSensivity ? x.ConfigName.ToLower() : x.ConfigName) == (!CaseSensivity ? configName.ToLower() : configName));
                    if (data != null)
                    {
                        if (index != splitPath.Length - 1)
                        {
                            if ((!CaseSensivity ? data.RootNode.NodeName.ToLower() : data.RootNode.NodeName) == (!CaseSensivity ? splitPath[index].ToLower() : splitPath[index]))
                            {
                                result = GetParameter_Rec(data.RootNode, CaseSensivity);
                            }
                        }
                    }
                }
            }
            return result;
        }

        //Recursive Method
        private string GetParameter_Rec(Node node, bool CaseSensivity = true)
        {
            string result = "";

            index++;
            if ((index) == (splitPath.Length - 1))
            {
                if (node.Attributes != null)
                {
                    for (int i = 0; i < node.Attributes.Length; i++)
                    {
                        if ((!CaseSensivity ? node.Attributes[i].AttributeName.ToLower() : node.Attributes[i].AttributeName) == (!CaseSensivity ? splitPath[index].ToLower() : splitPath[index]))
                        {
                            result = node.Attributes[i].Value;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (node.SubNodes != null)
                {
                    for (int i = 0; i < node.SubNodes.Length; i++)
                    {
                        if ((!CaseSensivity ? node.SubNodes[i].NodeName.ToLower() : node.SubNodes[i].NodeName) == (!CaseSensivity ? splitPath[index].ToLower() : splitPath[index]))
                        {
                            result = GetParameter_Rec(node.SubNodes[i], CaseSensivity);
                            if (result != "")
                                break;
                        }
                    }
                }
            }
            return result;
        }
    }
}