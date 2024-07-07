using UnityEngine;

[CreateAssetMenu(fileName = "SceneObjectsData", menuName = "ScriptableObjects/SceneObjectsData", order = 1)]
public class SceneObjectsData : ScriptableObject
{
    public GameObject[] homeSceneObjects;
    public GameObject[] gardenSceneObjects;
    public GameObject[] flowerShopSceneObjects;
}