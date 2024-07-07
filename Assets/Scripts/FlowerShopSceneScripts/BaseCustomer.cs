using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCustomer : MonoBehaviour
{
    [SerializeField] private GameObject preferencePanel;
    [SerializeField] private Transform panelPos;
    [SerializeField] private GameObject flowerOrderItem;
    [SerializeField] private GameObject moneyPanel;
    [SerializeField] private TextMeshProUGUI moneyEarnedText;
    [SerializeField] private Transform moneyPos;
    [SerializeField] private MoneyButton moneyButton;
    [SerializeField] private ControlWalking controlWalkingOfCustomer;

    private Camera mainCamera;
    private Transform initialPos;
    private bool isPreferenceShown = false;
    private CustomerPreference customerPreferenceSO;
    private int totalMoneyEarned = 0;
    private int totalExpPoint = 0;
    private bool isDone = false;

    private List<FlowerOrderItem> flowerOrderItems = new List<FlowerOrderItem>();

    void Start()
    {
        mainCamera = Camera.main;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(panelPos.position);
        preferencePanel.transform.position = screenPosition;
        foreach (var preference in customerPreferenceSO.flowerPreferences)
        {
            if (preference != null)
            {
                GameObject orderSpawned = Instantiate(flowerOrderItem, preferencePanel.transform);

                var itemIcon = orderSpawned.transform.Find("Image").GetComponent<Image>();
                var itemNumber = orderSpawned.transform.Find("Number").GetComponent<TextMeshProUGUI>();

                itemIcon.sprite = preference.flowerSO.icon;
                itemNumber.text = preference.quantity.ToString();

                flowerOrderItems.Add(new FlowerOrderItem
                {
                    preference = preference,
                    orderItem = orderSpawned
                });
            }      
        }
        
        MainMenuManager.Instance.OnSceneChanged += MainMenu_OnSceneChanged;
        moneyButton.OnCollectMoney += MoneyButton_OnCollectMoney;
    }

    private void MoneyButton_OnCollectMoney(object sender, System.EventArgs e)
    {
        if (!AudioManager.Instance.GetSPXSource().isPlaying)
        {
            AudioManager.Instance.PlaySPF(AudioManager.Instance.moneyPickup);
        }

        GameManager.Instance.AddMoney(totalMoneyEarned);
        GameManager.Instance.AddExpPoint(totalExpPoint);
    }

    private void MainMenu_OnSceneChanged(object sender, System.EventArgs e)
    {
        if (MainMenuManager.Instance.GetCurrentScene() != Scene.FlowerShopScene)
        {
            if (preferencePanel != null && preferencePanel.activeSelf)
            {
                preferencePanel.SetActive(false);
            }
        }
        else
        {
            if (preferencePanel != null && isPreferenceShown)
                preferencePanel.SetActive(true);
        }
    }

    void OnMouseDown()
    {
        if (!isDone && !controlWalkingOfCustomer.IsMoving())
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(panelPos.position);
            preferencePanel.transform.position = screenPosition;
            preferencePanel.SetActive(!preferencePanel.activeSelf);
            Debug.Log(isDone);
        }
    }

    public void SetCustomerPreferenceSO(CustomerPreference customerPreferenceSO)
    {
        this.customerPreferenceSO = customerPreferenceSO;
    }

    public CustomerPreference GetCustomerPreferenceSO()
    {
        return this.customerPreferenceSO;
    }

    private void HandleSelling(Pot pot)
    {
        if (Input.GetMouseButtonUp(0) && !controlWalkingOfCustomer.IsMoving())
        {
            foreach (var flowerOrderItem in flowerOrderItems)
            {
                if (pot.GetFlowerSOofPot() == flowerOrderItem.preference.flowerSO)
                {
                    if (flowerOrderItem.preference.quantity > 0)
                    {
                        flowerOrderItem.preference.quantity--;
                        totalMoneyEarned += flowerOrderItem.preference.flowerSO.value;
                        totalExpPoint += flowerOrderItem.preference.flowerSO.expPoint;
                        FlowerShopManager.Instance.RemoveFLowerOnShown(pot.GetFlowerSOofPot());
                        Destroy(pot.gameObject);
                        UpdateFlowerOrderItem(flowerOrderItem);
                        CheckOrderCompletion();
                    }
                }
            }
        }   
    }

    private void UpdateFlowerOrderItem(FlowerOrderItem flowerOrderItem)
    {
        var itemNumber = flowerOrderItem.orderItem.transform.Find("Number").GetComponent<TextMeshProUGUI>();
        itemNumber.text = flowerOrderItem.preference.quantity.ToString();

        if (flowerOrderItem.preference.quantity == 0)
        {
            var checkMark = flowerOrderItem.orderItem.transform.Find("CheckMark").gameObject;
            checkMark.SetActive(true);
        }
    }

    private void CheckOrderCompletion()
    {
        foreach (var flowerOrderItem in flowerOrderItems)
        {
            if (flowerOrderItem.preference.quantity > 0)
            {
                return;
            }
        }

        ShowMoneyPanel(totalMoneyEarned);
    }
    private void ShowMoneyPanel(float amount)
    {
        preferencePanel.gameObject.SetActive(false);
        moneyEarnedText.text = totalMoneyEarned.ToString();
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(moneyPos.position);
        moneyPanel.transform.position = screenPosition;
        moneyPanel.SetActive(true);
        isDone = true;
        Debug.Log("EXP: " + totalExpPoint);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Pot"))
        {
            Pot pot = collision.GetComponent<Pot>();
            if (pot.IsClicked())
                HandleSelling(pot);
        }
    }

    private class FlowerOrderItem
    {
        public FlowerPreference preference; //To handle flower and quatity
        public GameObject orderItem; //To handle checkMark
    }
}
