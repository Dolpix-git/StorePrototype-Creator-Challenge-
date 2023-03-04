using System;
using UnityEngine;

[Serializable]
public class ItemStackableObject{
    private ItemScriptableObject item;
    private int itemAmount;

    public ItemStackableObject(ItemScriptableObject item) {
        this.item = item;
    }

    public ItemScriptableObject Item { get => item; }
    public int ItemAmount { get => itemAmount; set => itemAmount = value; }

    public bool IsEmpty() {
        if (itemAmount <= 0) {
            return true;
        }
        return false;
    }
    public bool IsFull() {
        if (itemAmount >= item.itemMaxStack) {
            return true;
        }
        return false;
    }
    public int AttemptAddItem(int amount) {
        itemAmount += amount;
        int overflow = Mathf.Max(itemAmount - item.itemMaxStack,0);
        itemAmount = Mathf.Clamp(itemAmount, 0, item.itemMaxStack);
        return overflow;
    }
    public int AttemptRemoveItem(int amount) {
        itemAmount -= amount;
        int overflow = -Mathf.Min(itemAmount, 0);
        itemAmount = Mathf.Clamp(itemAmount, 0, item.itemMaxStack);
        return overflow;
    }
}
