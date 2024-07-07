using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private MenuPanel menuPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        MenuPanelManager.Instance.SelectPanel(menuPanel);
    }
}
