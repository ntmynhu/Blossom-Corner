using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowerButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI number;

    private FlowerSO flowerSO;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void Update()
    {
        UpdateFlowerButtonUI();
    }

    public void SetItemSO(FlowerSO itemSO)
    {
        this.flowerSO = itemSO;
    }

    public FlowerSO GetFlowerSO()
    {
        return flowerSO;
    }
    private void OnButtonClick()
    {
        if (MainMenuManager.Instance.GetCurrentScene() == Scene.FlowerShopScene && FlowerShopToolPanelManager.Instance.IsSelected())
        {
            MoveFromStorageToToolPanel();
        }
        else if (MainMenuManager.Instance.GetCurrentScene() == Scene.FlowerShopScene && this.transform.parent.gameObject == FlowerShopToolPanelManager.Instance.gameObject)
        {
            List<Pot> selectedPots = PotStandManager.Instance.GetSelectedPots();
            if (selectedPots != null)
            {
                int count = selectedPots.Count;
                int currentNumber = InventoryManager.Instance.GetNumberOfFlowerSO(flowerSO);

                if(currentNumber >= count)
                {
                    PotStandManager.Instance.PlaceFlower(flowerSO);
                }
            }
        }
    }

    private void UpdateFlowerButtonUI()
    {
        int currentNumber = InventoryManager.Instance.GetNumberOfFlowerSO(flowerSO);

        if (currentNumber == 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            number.text = currentNumber.ToString();
        }
    }

    private void MoveFromStorageToToolPanel()
    {
        foreach (Transform child in FlowerShopToolPanelManager.Instance.transform)
        {
            if (child.gameObject.GetComponent<FlowerButton>() != null)
            {
                if (child.gameObject.GetComponent<FlowerButton>().GetFlowerSO().id == this.flowerSO.id)
                {
                    return;
                }
            }

        }

        gameObject.transform.SetParent(FlowerShopToolPanelManager.Instance.transform);
        InventoryUIManager.Instance.ListItems();

        Transform xButton = gameObject.transform.Find("XButton");
        if (xButton != null)
        {
            xButton.gameObject.SetActive(true);
            //XButton is handled in flowerToolPanelManager
        }
    }

}
