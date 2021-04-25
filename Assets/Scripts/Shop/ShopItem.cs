using UnityEngine;

public abstract class ShopItem : ScriptableObject
{
    public int cost;
    public Sprite icon;
    public string itemName;
    public string itemDescription;
    public ShopItem requirement;
    public abstract void Apply();
}
