using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XMLDO
{
    public class RowHandler : MonoBehaviour
    {
        //*************************************************
        public RectTransform content;
        public RectTransform AttributerowPrefab;
        public RowHandler RowHandlerPrefab;

        private RectTransform currentAttributeRow;

        public List<RowHandler> subNodes = new List<RowHandler>();
        public List<InputField> attributes = new List<InputField>();

        public InputField inputField;

        public Text titleText;
        public RectTransform colorImageHandler;
        public Image colorImage;

        private string titleName;

        private bool contentState;
        //****************************************************    
        public void Set(Node node, Color color)
        {
            if (titleText != null)
            {
                titleText.text = node.NodeName;
                titleName = node.NodeName;
                if (node.SubNodes != null || node.Attributes != null)
                {
                    if (node.SubNodes.Length > 0 || node.Attributes.Length > 0)
                        titleText.text = "-" + titleName;
                }
            }
            if (colorImage != null)
            {
                Image newColorImage = Instantiate(colorImage, colorImageHandler);
                newColorImage.color = color;
                newColorImage.gameObject.SetActive(true);
            }
        }
        //Recursive Method
        public void CreateNode(Node nd, RectTransform targetContent, List<RowHandler> targetSubNodes, Color color)
        {
            RowHandler newHandler = Instantiate(RowHandlerPrefab, targetContent);
            newHandler.gameObject.SetActive(true);
            newHandler.Set(nd, color);

            targetSubNodes.Add(newHandler);

            if (nd.Attributes != null)
            {
                if (nd.Attributes.Length > 0)
                {
                    newHandler.CreateAttribute(nd.Attributes);
                }
            }
            if (nd.SubNodes != null)
            {
                float r = Random.Range(0f, 1f);
                float g = Random.Range(0f, 1f);
                float b = Random.Range(0f, 1f);
                Color newColor = new Color(r, g, b, 1);

                if (nd.SubNodes != null && nd.Attributes != null)
                {
                    if (nd.SubNodes.Length != 0 || nd.Attributes.Length != 0)
                        newHandler.Set(nd, newColor);
                }

                for (int i = 0; i < nd.SubNodes.Length; i++)
                {
                    CreateNode(nd.SubNodes[i], newHandler.content, newHandler.subNodes, newColor);
                }
            }
        }
        private void CreateAttribute(Attribute[] atrs)
        {
            if (atrs == null) return;

            currentAttributeRow = Instantiate(AttributerowPrefab, content);
            currentAttributeRow.gameObject.SetActive(true);
            for (int i = 0; i < atrs.Length; i++)
            {
                if (currentAttributeRow.childCount == 4)
                {
                    currentAttributeRow = Instantiate(AttributerowPrefab, content);
                    currentAttributeRow.gameObject.SetActive(true);
                }
                InputField newField = Instantiate(inputField, currentAttributeRow);
                newField.Set(atrs[i]);
                attributes.Add(newField);
            }
        }

        public void ContentState()
        {
            if (content.childCount > 0)
            {
                contentState = !contentState;
                if (contentState)
                    titleText.text = "+" + titleName;
                else
                    titleText.text = "-" + titleName;

                content.gameObject.SetActive(contentState);
            }
        }
    }
}