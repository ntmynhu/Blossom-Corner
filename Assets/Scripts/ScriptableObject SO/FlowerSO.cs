using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Flower", menuName = "Flower/Create New Flower")] 
public class FlowerSO : ScriptableObject
{
    public int id;
    public string flowerName;
    public int number;
    public Sprite icon;
    public GameObject flowerPrefab;
    public int value;
    public int expPoint;
}
