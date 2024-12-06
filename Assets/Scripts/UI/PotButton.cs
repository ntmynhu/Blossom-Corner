using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotButton : HasNumberButton
{
    #region Singleton
    private static PotButton instance;
    public static PotButton Instance => instance;

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

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        //numberText.text = currentNumber.ToString();
    }

    private void OnButtonClick()
    {
        if (MainMenuManager.Instance.GetCurrentScene() == Scene.FlowerShopScene && FlowerShopToolPanelManager.Instance.IsSelected())
        {
            gameObject.transform.SetParent(FlowerShopToolPanelManager.Instance.transform);

            InventoryManager.Instance.SetUpdate(true);

            Transform xButton = gameObject.transform.Find("XButton");
            if (xButton != null)
            {
                xButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (this.transform.parent.gameObject == FlowerShopToolPanelManager.Instance.gameObject)
            {
                if (currentNumber > 0 && PotStandManager.Instance.IsSelectingPotStand())
                {
                    PotStandManager.Instance.PlacePot();
                    UpdatePotButtonUI();
                }
            }
        }
    }

    public void UpdatePotButtonUI()
    {
        currentNumber--;
        numberText.text = currentNumber.ToString();
    }
}
