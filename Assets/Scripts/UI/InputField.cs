using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XMLDO
{
    public class InputField : MonoBehaviour
    {
        public Text title;
        public UnityEngine.UI.InputField input;

        private Attribute attribute;

        public string text { get { return input.text; } }

        public void Set(Attribute att)
        {
            attribute = att;
            input.text = attribute.Value;
            title.text = attribute.AttributeName;
        }

        public string GetValue() => input.text;
    }
}
