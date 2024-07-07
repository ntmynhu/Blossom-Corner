using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI priceText;
    private ShopItemSO item;

    public void Setup(ShopItemSO newItem)
    {
        item = newItem;
        itemIcon.sprite = item.itemIcon;
        priceText.text = item.price.ToString();
    }
}
