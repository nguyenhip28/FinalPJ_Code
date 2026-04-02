using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/Item")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public int price;
    public GameObject prefab;
    public Sprite icon;
    public ItemType type;
}

public enum ItemType
{
    Food,
    Furniture
}