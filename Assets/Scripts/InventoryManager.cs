using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    #region Singleton

    private static InventoryManager instance;
    public static InventoryManager Instance => instance;

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

    private List<FlowerSO> itemSOList = new List<FlowerSO>();
    private bool isUpdated = false;

    public void Add(FlowerSO item)
    {
        itemSOList.Add(item);
        isUpdated = true;
    }

    public void IncreaseNumber(FlowerSO item)
    {
        item.number++;
        isUpdated = true;
    }

    public void DecreaseNumber(FlowerSO item)
    {
        item.number--;
        isUpdated = true;
        if (item.number <= 0)
        {
            Remove(item);
        }
    }

    public void DecreaseNumber(FlowerSO item, int amount)
    {
        item.number -= amount;
        isUpdated = true;
        if (item.number <= 0)
        {
            Remove(item);
        }
    }

    public void Remove(FlowerSO item)
    {
        itemSOList.Remove(item);
        isUpdated = true;
    }

    public List<FlowerSO> GetItemSOList()
    {
        return itemSOList;
    }

    public int GetNumberOfFlowerSO(FlowerSO flowerSO)
    {
        return flowerSO.number;
    }

    public bool IsUpdated()
    {
        return isUpdated;
    }

    public void SetUpdate(bool isUpdated)
    {
        this.isUpdated = isUpdated;
    }

    public void LoadData(GameData data)
    {
        itemSOList.Clear();
        Debug.Log("Load inventory");
        foreach (InventoryFlower inventoryFlower in data.inventoryFlowers)
        {
            FlowerSO flowerSO = FlowerDatabase.Instance.GetFlowerByID(inventoryFlower.flowerID);
            if (flowerSO != null)
            {
                flowerSO.number = inventoryFlower.quantity;
                itemSOList.Add(flowerSO);
            }
            Debug.Log(flowerSO);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.inventoryFlowers.Clear();

        foreach (FlowerSO flowerSO in itemSOList)
        {
            InventoryFlower inventoryFlower = new InventoryFlower
            {
                flowerID = flowerSO.id,
                quantity = flowerSO.number
            };
            data.inventoryFlowers.Add(inventoryFlower);
        }
    }

}
