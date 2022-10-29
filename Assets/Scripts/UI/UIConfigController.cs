using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLDO
{
    public class UIConfigController : MonoBehaviour
    {
        public bool activeShortcut;

        public ConfigPanel configPanelPrefab;
        public UIButton panelButtonPrefab;
        public RectTransform buttonHandler;

        private List<ConfigData> datas;
        private ConfigPanel openingPanel;
        private Canvas canvas;

        private bool isOpen;
        //****************************************
        private void OnEnable()
        {
            datas = ConfigManager.ConfigIO.Instance.GetConfigDatas();

            if (datas == null)
                throw new System.Exception("Never added data! \n => UIConfigController.cs");

            canvas = GetComponent<Canvas>();

            if (activeShortcut)
            {
                canvas.enabled = false;
                isOpen = false;
            }
            else
            {
                canvas.enabled = true;
                isOpen = true;
            }

            CreatePanels();
        }

        private void Update()
        {
            if (activeShortcut)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        if (isOpen)
                        {
                            Exit();
                            isOpen = false;
                        }
                        else
                        {
                            Enter();
                            isOpen = true;
                        }
                    }
                }
            }
        }

        public void Enter()
        {
            if (canvas != null)
                canvas.enabled = true;
        }
        public void Exit()
        {
            if (canvas != null)
                canvas.enabled = false;
        }

        private void CreatePanels()
        {
            for (int i = 0; i < datas.Count; i++)
            {
                ConfigPanel newPanel = Instantiate(configPanelPrefab, transform);
                newPanel.SetUP(datas[i].ConfigName, datas[i].RootNode, datas[i]);
                newPanel.gameObject.SetActive(false);

                newPanel.uiButton = CreatePanelButton(newPanel);

                if (i == datas.Count - 1)
                    ShowPanel(newPanel);
            }
        }
        private UIButton CreatePanelButton(ConfigPanel cP)
        {
            UIButton newButton = Instantiate(panelButtonPrefab, buttonHandler.transform);
            newButton.Set(cP);
            newButton.OnClickEvent += ShowPanel;
            newButton.Deactive();

            return newButton;
        }

        private void ShowPanel(ConfigPanel cP)
        {
            if (cP != null)
            {
                if (openingPanel != null)
                {
                    openingPanel.CloseUIButton();
                    openingPanel.gameObject.SetActive(false);
                    openingPanel = cP;
                    openingPanel.gameObject.SetActive(true);
                }
                else
                {
                    openingPanel = cP;
                    openingPanel.OpenUIButton();
                    openingPanel.gameObject.SetActive(true);
                }
            }
        }
    }
}