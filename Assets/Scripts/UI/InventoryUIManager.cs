using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{

    #region Singleton

    private static InventoryUIManager instance;
    public static InventoryUIManager Instance => instance;

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

    [SerializeField] private GameObject inventoryItem;
    [SerializeField] private Transform itemContent;
    [SerializeField] private GameObject inventoryItemContainer;
    [SerializeField] private Transform toolPanel;

    private List<FlowerSO> itemSOList;

    public void Init()
    {
        InventoryManager.Instance.SetUpdate(true);
        if (InventoryManager.Instance.GetItemSOList() != null)
        {
            itemSOList = InventoryManager.Instance.GetItemSOList();
        }
        itemSOList = InventoryManager.Instance.GetItemSOList();
        ListItems();
    }

    private void Update()
    {
        if (InventoryManager.Instance.IsUpdated() /*&& inventoryItemContainer.activeSelf*/ && itemSOList != null)
        {
            ListItems();

            if (FlowerShopToolPanelManager.Instance != null)
            {
                FlowerShopToolPanelManager.Instance.ListInventoryItem();
            }

            InventoryManager.Instance.SetUpdate(false);
        }
    }

    public void ListItems()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in itemSOList)
        {
            GameObject obj = Instantiate(inventoryItem, itemContent);

            //var itemName = obj.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("Image").GetComponent<Image>();
            var itemNumber = obj.transform.Find("Number").GetComponent<TextMeshProUGUI>();

            //itemName.text = item.flowerName;
            itemIcon.sprite = item.icon;
            itemNumber.text = item.number.ToString();

            FlowerButton flowerButton = obj.GetComponent<FlowerButton>();
            if (flowerButton != null)
            {
                flowerButton.SetItemSO(item);
            }
        }
    }

    public void RemoveItemSO(FlowerSO flowerSO)
    {
        foreach (var flowerItemSO in itemSOList)
        {
            if (flowerSO == flowerItemSO)
            {
                itemSOList.Remove(flowerItemSO);
                break;
            }
        }
    }
}
