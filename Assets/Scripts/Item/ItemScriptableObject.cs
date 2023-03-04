using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject {
    public string itemName;
    public float itemCost;
    public float itemSell;
    public ItemCatagory itemCatagory;
    public float itemRarity;
    public int itemMaxStack;
}