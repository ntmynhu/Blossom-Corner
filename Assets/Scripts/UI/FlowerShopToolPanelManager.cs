using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class FlowerShopToolPanelManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    #region Singleton

    private static FlowerShopToolPanelManager instance;
    public static FlowerShopToolPanelManager Instance => instance;

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

    [SerializeField] private Transform inventory;
    [SerializeField] private GameObject tickMark;
    [SerializeField] private GameObject inventoryItem;

    public Color selectedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    public Color normalColor = Color.white;

    private bool isHolding = false;
    private bool isSelected = false;
    private float timeToSelectMax = 1f;
    private float timeToSelect;

    private void Start()
    {
        ListInventoryItem();
        timeToSelect = timeToSelectMax;
        MainMenuManager.Instance.OnSceneChanged += MainMenu_OnSceneChanged;
    }

    private void Update()
    {
        if (isHolding && !isSelected)
        {
            timeToSelect -= Time.deltaTime;
            if (timeToSelect < 0)
            {
                isSelected = true;
                this.GetComponent<Image>().color = selectedColor;
                ShowXButtons(); // Show X buttons when the panel is selected
                ShowTickMark();
            }
        }
        else if (!isHolding && !isSelected)
        {
            timeToSelect = timeToSelectMax;
        }

    }

    public void ListInventoryItem()
    {
        foreach (Transform item in gameObject.transform)
        {
            if (!item.gameObject.CompareTag("PotButton"))
            {
                Destroy(item.gameObject);
            }
        }

        List<FlowerSO> flowerSOList = InventoryManager.Instance.GetItemSOList();
        foreach (var item in flowerSOList)
        {
            GameObject obj = Instantiate(inventoryItem, gameObject.transform);

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

    private void MainMenu_OnSceneChanged(object sender, System.EventArgs e)
    {
        if (MainMenuManager.Instance.GetCurrentScene() != Scene.FlowerShopScene)
        {
            ResetSelection();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    private void ShowTickMark()
    {
        tickMark.SetActive(true);
        tickMark.GetComponent<Button>().onClick.AddListener(OnTickMarkClick);
    }

    private void OnTickMarkClick()
    {
        ResetSelection();
        tickMark.SetActive(false);
    }

    private void ShowXButtons()
    {
        foreach (Transform child in transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                // Show X button for each child button
                Transform xButton = child.Find("XButton");
                if (xButton != null)
                {
                    xButton.gameObject.SetActive(true);
                    Button xButtonComponent = xButton.GetComponent<Button>();
                    if (xButtonComponent != null)
                    {
                        // Attach a listener to the X button
                        xButtonComponent.onClick.AddListener(() => OnXButtonClick(child.gameObject));
                    }
                }
            }
        }
    }

    private void ResetSelection()
    {
        isSelected = false;
        timeToSelect = timeToSelectMax;
        foreach (Transform child in transform)
        {
            Transform xButton = child.Find("XButton");
            if (xButton != null)
            {
                xButton.gameObject.SetActive(false);
            }
        }
        this.GetComponent<Image>().color = normalColor;
        tickMark.SetActive(false);
    }

    private void OnXButtonClick(GameObject buttonObject)
    {
        if (buttonObject.CompareTag("PotButton"))
        {
            // Remove button from tool panel and add it back to the inventory
            buttonObject.transform.SetParent(inventory);

            // Hide X button in the tool panel
            Transform xButton = buttonObject.transform.Find("XButton");
            if (xButton != null)
            {
                xButton.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(buttonObject);
        }
        
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}
