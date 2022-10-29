using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XMLDO
{
    public class ConfigPanel : MonoBehaviour
    {
        public Text title;

        [HideInInspector]
        public ConfigData data;

        public RowHandler rowHandler;
        private RowHandler currentRowHandler;

        public RectTransform rowContent;

        public UIButton uiButton { private get; set; }
        //*****************************************************
        public void SetUP(string titleText, Node _node, ConfigData _data)
        {
            if (_data != null)
                data = _data;
            if (title != null)
                title.text = titleText;
            CreateRow_Rec(_node);
        }
        //Recursive Method
        private void CreateRow_Rec(Node node)
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            Color newColor = new Color(r, g, b, 1);

            RowHandler newRowHandler = Instantiate(rowHandler, rowContent);
            newRowHandler.Set(node, newColor);
            newRowHandler.gameObject.SetActive(true);
            if (node.SubNodes != null)
            {
                if (node.SubNodes.Length > 0)
                {
                    for (int i = 0; i < node.SubNodes.Length; i++)
                    {
                        newRowHandler.CreateNode(node.SubNodes[i], newRowHandler.content, newRowHandler.subNodes, newColor);
                    }
                }
            }
            currentRowHandler = newRowHandler;
        }

        public void Save()
        {
            if (data == null || currentRowHandler == null) return;

            ConfigManager.ConfigIO.Instance.SaveData(data.ConfigName, currentRowHandler);
        }
        public void Export()
        {
            if (data == null || currentRowHandler == null) return;

            ConfigManager.ConfigIO.Instance.SaveData(data.ConfigName, currentRowHandler);
            ConfigManager.ConfigCore.Instance.ExportXML(data);
        }

        public void CloseUIButton()
        {
            if (uiButton != null)
                uiButton.Deactive();
        }
        public void OpenUIButton()
        {
            if (uiButton != null)
                uiButton.Active();
        }
    }
}