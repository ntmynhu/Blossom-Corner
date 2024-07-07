using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    #region Singleton

    private static GardenManager instance;
    public static GardenManager Instance => instance;

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

    /*private Dictionary<string, BaseFlower> flowersOnGarden = new Dictionary<string, BaseFlower>();

    public void AddFLower(BaseFlower flower)
    {
        flowersOnGarden.Add(flower.GetId(), flower);
    }

    public void RemoveFLower(BaseFlower flower)
    {
        flowersOnGarden.Remove(flower.GetId());
    }*/
}
