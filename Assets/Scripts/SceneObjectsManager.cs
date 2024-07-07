using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsManager : MonoBehaviour
{
    [SerializeField] SceneObjectsData sceneObjectsData;

    #region Singleton

    private static SceneObjectsManager instance;
    public static SceneObjectsManager Instance => instance;

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

    public GameObject[] GetHomeSceneObjects()
    {
        return sceneObjectsData.homeSceneObjects;
    }

    public GameObject[] GetGardenSceneObjects()
    {
        return sceneObjectsData.gardenSceneObjects;
    }

    public GameObject[] GetFlowerShopSceneObjects()
    {
        return sceneObjectsData.flowerShopSceneObjects;
    }
}

