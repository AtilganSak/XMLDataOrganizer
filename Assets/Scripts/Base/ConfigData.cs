using System.Collections.Generic;
using UnityEngine;

namespace XMLDO
{
    [CreateAssetMenu(fileName = "NewConfigData", menuName = "Create New Config Data")]
    public class ConfigData : ScriptableObject
    {
        public string ConfigName;
        public string ParentFolder;

        public Node RootNode;
    }
    [System.Serializable]
    public struct Node
    {
        public string NodeName;
        public Attribute[] Attributes;
        public Node[] SubNodes;
    }
    [System.Serializable]
    public struct Attribute
    {
        public string AttributeName;
        public string Value;
    }
}