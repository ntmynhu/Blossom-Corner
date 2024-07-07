using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlowerState
{
    public string flowerID;
    public int currentStateIndex;
    public float currentGrowthTime;
    public Vector3 position;
    public int grassAmount;
    public float totalGrowingTime;
}

[Serializable]
public class PlantSpotSateData
{
    public string flowerPrefabPath;
    public PlantSpotState state;
    public string flowerID;
}

[Serializable]
public class InventoryFlower
{
    public int flowerID;
    public int quantity;
}

[Serializable]
public class CustomerOrder
{
    public int flowerID;
    public int quantity;
}

[Serializable]
public class CustomerState
{
    public string customerName;
    public List<CustomerOrder> orders;
    public float timeLeft;
}

[Serializable]
public class GameData
{
    public int playerMoney;
    public int playerEXPPoint;
    public int playerEXPPointMax;
    public int playerLevel;
    public string playerName;

    public SerializableDictionary<string, int> hasNumberButtons;
    public List<InventoryFlower> inventoryFlowers;
    public SerializableDictionary<string, PlantSpotSateData> gardenPlantSpots;
    public SerializableDictionary<string, FlowerState> flowerStates;
    
    /*public List<CustomerState> shopCustomers;
    public List<CustomerOrder> orderList;*/

    public GameData()
    {
        playerName = string.Empty;
        playerMoney = 1000;
        playerEXPPointMax = 30;
        playerEXPPoint = 0;
        playerLevel = 1;

        hasNumberButtons = new SerializableDictionary<string, int>();
        inventoryFlowers = new List<InventoryFlower>();
        gardenPlantSpots = new SerializableDictionary<string, PlantSpotSateData>();
        flowerStates = new SerializableDictionary<string, FlowerState>();

        /*shopCustomers = new List<CustomerState>();
        orderList = new List<CustomerOrder>();*/
    }
}
