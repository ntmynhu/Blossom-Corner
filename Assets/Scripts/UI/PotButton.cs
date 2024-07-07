using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotButton : HasNumberButton
{
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
                    UpdatePotButtonUI(ref currentNumber);
                }
            }
        }
    }

    private void UpdatePotButtonUI(ref int currentNumber)
    {
        currentNumber--;
        numberText.text = currentNumber.ToString();
    }
}
