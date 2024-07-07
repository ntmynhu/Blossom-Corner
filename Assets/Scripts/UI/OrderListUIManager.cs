using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderListUIManager : MonoBehaviour
{

    #region Singleton

    private static OrderListUIManager instance;
    public static OrderListUIManager Instance => instance;

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

    [SerializeField] private Transform orderListPos;
    [SerializeField] private GameObject orderPanelPrefab;
    [SerializeField] private GameObject orderItemPrefab;
    [SerializeField] private TextMeshProUGUI numberOfOrderText;

    private Dictionary<string, CustomerPreference> orderDict = new Dictionary<string, CustomerPreference>();

    public void RemovePanel(string id)
    {
        if (orderDict.ContainsKey(id))
        {
            orderDict.Remove(id);
        }
        UpdateOrderListUI();
    }

    public void UpdateOrderListUI()
    { 
        foreach (Transform order in orderListPos)
        {
            Destroy(order.gameObject);
        }

        orderDict = FlowerShopManager.Instance.GetOrderDict();
        foreach (var order in orderDict)
        {
            GameObject orderPanelSpawned = Instantiate(orderPanelPrefab, orderListPos);
            OrderPanel orderPanel = orderPanelSpawned.GetComponent<OrderPanel>();

            orderPanel.SetId(order.Key);

            Transform preferencePos = orderPanelSpawned.transform.Find("OrderViewport");

            foreach (var preference in order.Value.flowerPreferences)
            {
                if (preference != null)
                {
                    GameObject orderItemSpawned = Instantiate(orderItemPrefab, preferencePos);

                    var itemIcon = orderItemSpawned.transform.Find("Image").GetComponent<Image>();
                    var itemNumber = orderItemSpawned.transform.Find("Number").GetComponent<TextMeshProUGUI>();

                    /*Debug.Log(itemIcon.sprite);
                    Debug.Log(preference.flowerSO.icon);*/
                    itemIcon.sprite = preference.flowerSO.icon;
                    itemNumber.text = preference.quantity.ToString();

                    orderPanel.AddFlowerOrderItem(preference, orderItemSpawned);
                }   
            }
        }

        numberOfOrderText.text = FlowerShopManager.Instance.GetNumberOfOrder().ToString();
    }
}
