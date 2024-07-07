using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private ShopItemSO shopItemSO;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI price;

    //[SerializeField] private HasNumberButton hasNumberButton;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        image.sprite = shopItemSO.itemIcon;
        price.text = shopItemSO.price.ToString();
    }

    private void OnButtonClick()
    {
        int currentMoney = GameManager.Instance.GetCurrentMoney();
        if (currentMoney >= shopItemSO.price)
        {
            currentMoney -= shopItemSO.price;

            AudioManager.Instance.PlaySPF(AudioManager.Instance.moneyPurchase);

            GameManager.Instance.SetCurrentMoney(currentMoney);
            ShopManager.Instance.Purchase(shopItemSO.toolType);
        }
    }
}
