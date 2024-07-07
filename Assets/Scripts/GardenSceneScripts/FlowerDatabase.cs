using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerDatabase : MonoBehaviour
{
    [SerializeField] private FlowerSO[] flowers;
    private Dictionary<int, FlowerSO> flowerDictionary = new Dictionary<int, FlowerSO>();

    #region Singleton

    private static FlowerDatabase instance;
    public static FlowerDatabase Instance => instance;

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

        foreach (FlowerSO flower in flowers)
        {
            flowerDictionary[flower.id] = flower;
        }
    }

    #endregion

    public FlowerSO GetFlowerByID(int id)
    {
        flowerDictionary.TryGetValue(id, out FlowerSO flower);
        return flower;
    }
}
