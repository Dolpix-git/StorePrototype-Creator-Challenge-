using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class Shop{
    private List<ItemStackableObject> shopInventory = new List<ItemStackableObject>();
    private float shopkeepMoney = 0;

    public event Action<List<ItemStackableObject>> OnInventoryUpdate;
    public event Action<float> OnMoneyUpdate;

    public void SellItem(ItemScriptableObject item, int amount) {
        Debug.Log(amount);
        int amountWeCanSell = amount - Player.Instance.AttemptToRemoveItem(item,amount);
        Debug.Log(amountWeCanSell);
        if (amountWeCanSell == 0) {
            return;
        }
        int amountKeepCanBuy = Mathf.FloorToInt(shopkeepMoney/(amountWeCanSell*item.itemSell));
        Debug.Log(amountKeepCanBuy);
        if (amountKeepCanBuy == 0) {
            Player.Instance.AttemptToAddItem(item, amount);
            return;
        }
        int amountWeGoWith = Mathf.Min(amountWeCanSell,amountKeepCanBuy);
        Debug.Log(amountWeGoWith);
        if (amountWeGoWith < amount) {
            Player.Instance.AttemptToAddItem(item, amount-amountWeGoWith);
        }

        Player.Instance.ChangePlayerMoney(amountWeGoWith * item.itemSell);
        shopkeepMoney -= amountWeGoWith * item.itemSell;
        OnMoneyUpdate?.Invoke(shopkeepMoney);
    }
    public void BuyItem(ItemScriptableObject item, int amount) {
        int possibleAmountToBuy = Mathf.FloorToInt(Player.Instance.PlayerMoney / item.itemCost);
        amount = Mathf.Min(amount, possibleAmountToBuy);
        
        Debug.Log($"you have bought Name:{item.name} Cost:{-amount * item.itemCost} Amount:{amount}");

        int attempted = amount;
        amount = AttemptToRemoveItem(item,amount);
        Debug.Log(attempted + " " + amount);
        Player.Instance.AttemptToAddItem(item, attempted - amount);
        Player.Instance.ChangePlayerMoney(-(attempted - amount) * item.itemCost);
        if (amount == 0) { return; }
        Debug.Log($"Shopkeep ran out of stuff to give so he is refunding you {amount * item.itemCost}");
    }
    public void GenerateShop() {
        shopInventory.Clear();
        shopkeepMoney = Random.Range(1, 100);
        Debug.Log(ShopManager.Instance.ShopItems.Count);
        foreach (ItemScriptableObject item in ShopManager.Instance.ShopItems) {
            Debug.Log(item.name);
            AttemptToAddItem(item, Random.Range(1,100));
        }
        OnMoneyUpdate?.Invoke(shopkeepMoney);
    }
    private void AttemptToAddItem(ItemScriptableObject item, int amount) {
        foreach (ItemStackableObject shopItem in shopInventory) {
            if (!shopItem.IsFull()) {
                if (shopItem.Item == item) {
                    amount = shopItem.AttemptAddItem(amount);
                }
            }
        }

        if (amount == 0) {
            OnInventoryUpdate?.Invoke(shopInventory);
            return;
        }else if (amount < 0) {
            Debug.Log($"Somthing terible has happend when adding an amount to the shop inventory remaining:{amount}");
        } else {
            while (amount > 0) {
                ItemStackableObject stack = new ItemStackableObject(item);
                amount = stack.AttemptAddItem(amount);
                shopInventory.Add(stack);
            }
        }
        OnInventoryUpdate?.Invoke(shopInventory);
    }
    private int AttemptToRemoveItem(ItemScriptableObject item, int amount) {
        foreach (ItemStackableObject shopItem in shopInventory) {
            if (shopItem.Item == item) {
                amount = shopItem.AttemptRemoveItem(amount);
                Debug.Log(amount);
                if (shopItem.IsEmpty()) {
                    shopInventory.Remove(shopItem);
                }
                if (amount == 0) {
                    OnInventoryUpdate?.Invoke(shopInventory);
                    return 0;
                }
            }
        }
        OnInventoryUpdate?.Invoke(shopInventory);
        return amount;
    }
}
