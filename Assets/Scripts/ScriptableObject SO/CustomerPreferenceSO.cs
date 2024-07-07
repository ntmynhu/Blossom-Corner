using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomerPreference", menuName = "Customer/Preference")]
public class CustomerPreference : ScriptableObject
{
    public string customerName;
    public int minFlowers;
    public int maxFlowers;
    public FlowerPreference[] flowerPreferences;
    public GameObject prefab;
}

[System.Serializable]
public class FlowerPreference
{
    public FlowerSO flowerSO;
    public int quantity;
}
