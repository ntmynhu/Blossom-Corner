using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MenuPanel
{
    None,
    Storage,
    Shop,
    Map,
}

public class MenuPanelManager : MonoBehaviour
{
    #region Singleton

    private static MenuPanelManager instance;
    public static MenuPanelManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion


    [SerializeField] private GameObject storagePanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject mapPanel;

    private MenuPanel currentMenuPanel = MenuPanel.None;
    private GameObject currentPanel;

    private bool isShowingPanel = false;

    private void Start()
    {
        MainMenuManager.Instance.OnSceneChanged += MainMenu_OnSceneChanged;
    }

    private void MainMenu_OnSceneChanged(object sender, System.EventArgs e)
    {
        ChangePanel(MenuPanel.None);
    }

    public void SelectPanel(MenuPanel MenuPanel)
    {
        if (MenuPanel == currentMenuPanel)
        {
            ShowPanel();
        }
        else
        {
            ChangePanel(MenuPanel);
            ShowPanel();
        }

        isShowingPanel = currentPanel != null;
    }

    public void DeSelectPanel()
    {
        currentMenuPanel = MenuPanel.None;
        isShowingPanel = false;
    }

    private void ChangePanel(MenuPanel MenuPanel)
    {
        currentMenuPanel = MenuPanel;
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        switch (MenuPanel)
        {
            case MenuPanel.Storage:
                currentPanel = storagePanel;
                break;
            case MenuPanel.Shop:
                currentPanel = shopPanel;
                break;
            case MenuPanel.Map:
                currentPanel = mapPanel;
                break;
        }

    }

    private void ShowPanel()
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(!currentPanel.activeSelf);
            isShowingPanel = currentPanel.activeSelf;
        }
    }
}
