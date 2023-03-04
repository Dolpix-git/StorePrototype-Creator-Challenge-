using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour{
    private List<ItemStackableObject> shopInventory = new List<ItemStackableObject>();
    private float shopkeepMoney = 0;
    public void SellItem(ItemScriptableObject item, int amount) { // TOO DO: Waiting for the player to have money
        // loop through and have the shop keep buy as much of that item as possible
        // if shopkeep runs out of money early return
        // for every sucsessfull sale, give the money to the player
    }
    public void BuyItem(ItemScriptableObject item, int amount) { // TOO DO: Waiting for the player to have money
        // Take away money

        // if not possible they cant buy anything

        // else loop through shop items
        // if we loop through and are able to buy all the stuff we wanted exit

        // else the shop ran out of what we were looking for
    }
    public void GenerateShop() {
        shopInventory.Clear();
        shopkeepMoney = Random.Range(1, 100);
        foreach (ItemScriptableObject item in ShopManager.Instance.ShopItems) {
            AttemptToAddItem(item, Random.Range(1,100));
        }
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
    }
}
