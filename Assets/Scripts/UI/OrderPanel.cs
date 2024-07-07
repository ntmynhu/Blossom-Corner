using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelCheckMark;
    [SerializeField] private OrderCheckMark orderCheckMark;

    private string id;
    private List<FlowerOrderItem> flowerOrderItems = new List<FlowerOrderItem>();

    private void Start()
    {
        orderCheckMark.OnOrderCheckMarkClicked += OrderCheckMark_OnOrderCheckMarkClicked;
    }
    
    private void Update()
    {
        CheckOrderCompletion();
    }

    public void SetId(string id)
    {
        this.id = id;
    }


    private void OrderCheckMark_OnOrderCheckMarkClicked(object sender, System.EventArgs e)
    {
        int totalMoney = 0;
        int totalExpPoint = 0;
        foreach (var item in flowerOrderItems)
        {
            InventoryManager.Instance.DecreaseNumber(item.preference.flowerSO, item.preference.quantity);
            totalMoney += item.preference.flowerSO.value * item.preference.quantity;
            totalExpPoint += item.preference.flowerSO.expPoint * item.preference.quantity;
        }

        FlowerShopManager.Instance.DecreaseOrderNumber();
        GameManager.Instance.AddMoney(totalMoney);
        GameManager.Instance.AddExpPoint(totalExpPoint);
        OrderListUIManager.Instance.RemovePanel(id);
    }

    private void CheckOrderCompletion()
    {
        bool isCompleted = true;
        List<FlowerSO> inventoryFlowerSOList = InventoryManager.Instance.GetItemSOList();

        foreach (var flowerOrderItem in flowerOrderItems)
        {
            bool isFound = false;
            foreach (FlowerSO flowerSO in inventoryFlowerSOList)
            {
                if (flowerOrderItem.preference.flowerSO == flowerSO)
                {
                    if (flowerOrderItem.preference.quantity <= flowerSO.number)
                    {
                        var checkMark = flowerOrderItem.orderItem.transform.Find("CheckMark").gameObject;
                        checkMark.SetActive(true);  
                        isFound = true;
                    }
                    else
                    {
                        isCompleted = false;
                    }
                }
            }
            
            if (!isFound)
            {
                isCompleted = false;
            }
        }

        if (isCompleted)
        {
            panelCheckMark.SetActive(true);
        }
        else
        {
            panelCheckMark.SetActive(false);
        }

        Debug.Log("IsCompleted: " + isCompleted);
    }

    public void AddFlowerOrderItem(FlowerPreference flowerPreference, GameObject orderItem)
    {
        flowerOrderItems.Add(new FlowerOrderItem
        {
            preference = flowerPreference,
            orderItem = orderItem
        });
    }

    private class FlowerOrderItem
    {
        public FlowerPreference preference; //To handle flower and quatity
        public GameObject orderItem; //To handle checkMark
    }
}
