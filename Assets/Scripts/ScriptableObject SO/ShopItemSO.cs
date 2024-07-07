using UnityEngine;

[CreateAssetMenu(fileName = "New ShopItem", menuName = "Shop/Create new shop item")]
public class ShopItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int price;
    public ToolType toolType;
}
