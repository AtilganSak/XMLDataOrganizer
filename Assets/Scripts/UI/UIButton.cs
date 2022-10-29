using UnityEngine;
using UnityEngine.UI;

namespace XMLDO
{
    public class UIButton : MonoBehaviour
    {
        public Text nameText;

        public Button button;

        public delegate void OnClickDelegate(ConfigPanel cp);
        public event OnClickDelegate OnClickEvent;

        private ConfigPanel configPanel;

        private bool isActive;

        private void OnEnable()
        {
            if (button != null)
                button.onClick.AddListener(OnClick);
        }

        public void Set(ConfigPanel cP)
        {
            if (nameText != null)
                nameText.text = cP.data.ConfigName;
            configPanel = cP;
        }

        public void OnClick()
        {
            if (!isActive)
            {
                Active();

                if (OnClickEvent != null)
                    OnClickEvent.Invoke(configPanel);

                if (configPanel != null)
                    configPanel.gameObject.SetActive(true);
            }
        }
        public void Active()
        {
            if (button != null)
                button.GetComponent<Image>().color = new Color(1, 1, 1, 1f);

            isActive = true;
        }
        public void Deactive()
        {
            if (button != null)
                button.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            isActive = false;
        }
    }
}