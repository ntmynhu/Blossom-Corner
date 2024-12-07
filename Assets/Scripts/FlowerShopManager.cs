using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlowerShopManager : MonoBehaviour
{
    #region Singleton

    private static FlowerShopManager instance;
    public static FlowerShopManager Instance => instance;

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

    [SerializeField] private CustomerPreference[] customerTemplates;
    [SerializeField] private FlowerSO[] allFlowers;
    [SerializeField] private Transform customerInitialPos;

    private Dictionary<string, CustomerPreference> orderDict;
    private Dictionary<FlowerSO, int> flowerOnShownDict;
    private int numberOfCustomer = 0;
    private float customerSpawningTimeMax;
    private float customerSpawningTime;

    private int numberOfOrder = 0;
    private float orderSpawningTimeMax;
    private float orderSpawningTime;

    private int numberOfCustomerOnSceneMax = 1;
    private int numberOfOrderOnSceneMax = 2;

    void Start()
    {
        orderDict = new Dictionary<string, CustomerPreference>();
        flowerOnShownDict = new Dictionary<FlowerSO, int>();

        customerSpawningTimeMax = GetRandomCustomereSpawningWaitingTime();
        customerSpawningTime = customerSpawningTimeMax;

        /*orderSpawningTimeMax = GetRandomCustomereSpawningWaitingTime();
        orderSpawningTime = orderSpawningTimeMax;*/
    }

    private void Update()
    {
        HandleCustomer();
        HandleSpawningOrders();
    }

    private void HandleCustomer()
    {
        if (AreThereFlowersOnShop() && numberOfCustomer < numberOfCustomerOnSceneMax)
        {
            customerSpawningTime -= Time.deltaTime;
            if (customerSpawningTime <= 0)
            {
                SpawnCustomer();
                customerSpawningTimeMax = GetRandomCustomereSpawningWaitingTime();
                customerSpawningTime = customerSpawningTimeMax;

                if (GameManager.Instance.IsFirstTimePlayer())
                {
                    if (TutorialManager.Instance.GetCurrentStepIndex() == 15)
                    {
                        TutorialManager.Instance.OnActionCompleted(2f);
                    }
                }
            }
        }
    }

    private void HandleSpawningOrders()
    {
        if (GameManager.Instance.GetPlayerLevel() >= 2 && numberOfOrder < numberOfOrderOnSceneMax)
        {
            orderSpawningTime -= Time.deltaTime;
            if (orderSpawningTime <= 0)
            {
                SpawnOrder();
                OrderListUIManager.Instance.UpdateOrderListUI();
                orderSpawningTimeMax = GetRandomOrderSpawningWaitingTime();
                orderSpawningTime = orderSpawningTimeMax;
            }
        }
    }    

    private void SpawnOrder()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();

        int maxCustomer = Mathf.Min(playerLevel, customerTemplates.Length);
        var template = customerTemplates[Random.Range(0, maxCustomer)];

        var customerPreferenceSO = GenerateCustomerPreference(template, playerLevel);

        string id = Guid.NewGuid().ToString();
        orderDict[id] = customerPreferenceSO;
        numberOfOrder++;

        Debug.Log("Order Preference Created: " + customerPreferenceSO.customerName);
        foreach (var preference in customerPreferenceSO.flowerPreferences)
        {
            if (preference != null)
            {
                Debug.Log("Prefers " + preference.quantity + " of " + preference.flowerSO.flowerName);
            }
        }


    }    

    private void SpawnCustomer()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();

        int maxCustomer = Mathf.Min(playerLevel, customerTemplates.Length);
        var template = customerTemplates[Random.Range(0, maxCustomer)];

        var customerPreferenceSO = GenerateCustomerPreference(template, playerLevel);
        //currentPreferences.Add(customerPreferenceSO);
        numberOfCustomer++;

        Debug.Log("Customer Preference Created: " + customerPreferenceSO.customerName);
        foreach (var preference in customerPreferenceSO.flowerPreferences)
        {
            if (preference != null)
            {
                Debug.Log("Prefers " + preference.quantity + " of " + preference.flowerSO.flowerName);
            }
        }

        GameObject customerGObj = Instantiate(customerPreferenceSO.prefab, customerInitialPos);
        AudioManager.Instance.PlaySPF(AudioManager.Instance.doorBell);
        BaseCustomer baseCustomerGObj = customerGObj.GetComponent<BaseCustomer>();
        baseCustomerGObj.SetCustomerPreferenceSO(customerPreferenceSO);
    }

    private CustomerPreference GenerateCustomerPreference(CustomerPreference template, int level)
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        CustomerPreference customerPreference = Instantiate(template);
        customerPreference.customerName = template.customerName;

        int min = template.minFlowers;
        int max = template.maxFlowers;
        int flowerCount = Random.Range(min, max + 1);

        customerPreference.flowerPreferences = new FlowerPreference[flowerCount];
        int index = 0;

        List<FlowerSO> chosenFlowersList = new List<FlowerSO>();

        for (int i = 0; i < flowerCount; i++)
        {
            FlowerPreference flowerPreference = new FlowerPreference();
            int maxFlowersLength = Mathf.Min(playerLevel, allFlowers.Length);

            FlowerSO chosenFlower = allFlowers[Random.Range(0, maxFlowersLength)];

            if (chosenFlowersList.Contains(chosenFlower))
            {
                continue;
            }

            chosenFlowersList.Add(chosenFlower);

            flowerPreference.flowerSO = chosenFlower;
            flowerPreference.quantity = Random.Range(1, 2 * playerLevel);
            customerPreference.flowerPreferences[index] = flowerPreference;
            index++;
        }

        return customerPreference;
    }

    public void AddFlowerOnShown(FlowerSO flowerSO)
    {
        if (flowerOnShownDict.ContainsKey(flowerSO))
        {
            flowerOnShownDict[flowerSO]++;
        }
        else
        {
            flowerOnShownDict[flowerSO] = 1;
        }
    }

    public void RemoveFLowerOnShown(FlowerSO flowerSO)
    {
        if (flowerOnShownDict.ContainsKey(flowerSO))
        {
            flowerOnShownDict[flowerSO]--;
            if (flowerOnShownDict[flowerSO] <= 0)
            {
                flowerOnShownDict.Remove(flowerSO);
            }
        }
    }

    public void DecreaseCustomerNumber()
    {
        numberOfCustomer--;
    }

    public void DecreaseOrderNumber()
    {
        numberOfOrder--;
    }

    public bool AreThereFlowersOnShop()
    {
        foreach (var count in flowerOnShownDict.Values)
        {
            if (count > 0)
            {
                return true;
            }
        }
        return false;
    }

    private float GetRandomCustomereSpawningWaitingTime()
    {
        return Random.Range(1f, 20f);
    }

    private float GetRandomOrderSpawningWaitingTime()
    {
        return Random.Range(30f, 60f);
    }

    public Dictionary<string, CustomerPreference> GetOrderDict()
    {
        return orderDict;
    }

    public int GetNumberOfOrder()
    {
        return numberOfOrder;
    }
}
