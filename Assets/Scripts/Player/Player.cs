using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData {
    public ItemStackableObject[] playerInventory = new ItemStackableObject[0];
    public float playerMoney = 100;
}
public class Player : MonoBehaviour{
    private PlayerData playerData = new PlayerData();
    private List<ItemStackableObject> playerInventory = new List<ItemStackableObject>();
    private float playerMoney = 100;

    public event Action<List<ItemStackableObject>> OnInventoryUpdate;
    public event Action<float> OnMoneyUpdate;

    public static Player Instance { get; private set; }
    public float PlayerMoney { get => playerMoney; }
    public List<ItemStackableObject> PlayerInventory { get => playerInventory; }
    public PlayerData PlayerData { get => playerData; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        playerData = PlayerLoadSave.LoadPlayer();
        if (playerData != null) {
            playerMoney = playerData.playerMoney;
            OnMoneyUpdate?.Invoke(playerMoney);
            if (playerData.playerInventory != null) {
                playerInventory = playerData.playerInventory.ToList();
                OnInventoryUpdate?.Invoke(PlayerInventory);
            }
        }
    }
    private void OnApplicationQuit() {
        playerData = new PlayerData();

        PlayerData.playerMoney = playerMoney;
        PlayerData.playerInventory = playerInventory.ToArray();

        PlayerLoadSave.SavePlayer();
    }

    public void AttemptToAddItem(ItemScriptableObject item, int amount) {
        Debug.Log($"Attempting to add {item.name} Amount:{amount}");
        foreach (ItemStackableObject playerItem in playerInventory) {
            if (!playerItem.IsFull()) {
                if (playerItem.Item == item) {
                    amount = playerItem.AttemptAddItem(amount);
                }
            }
        }

        if (amount == 0) {
            OnInventoryUpdate?.Invoke(playerInventory);
            return;
        } else if (amount < 0) {
            Debug.Log($"Somthing terible has happend when adding an amount to the shop inventory remaining:{amount}");
        } else {
            while (amount > 0) {
                ItemStackableObject stack = new ItemStackableObject(item);
                amount = stack.AttemptAddItem(amount);
                playerInventory.Add(stack);
            }
        }
        OnInventoryUpdate?.Invoke(playerInventory);
    }
    public int AttemptToRemoveItem(ItemScriptableObject item, int amount) {
        Debug.Log($"Attempting to remove {item.name} Amount:{amount}");
        foreach (ItemStackableObject playerItem in playerInventory) {
            if (playerItem.Item == item) {
                amount = playerItem.AttemptRemoveItem(amount);
                if (playerItem.IsEmpty()) {
                    playerInventory.Remove(playerItem);
                }
                if (amount == 0) {
                    OnInventoryUpdate?.Invoke(playerInventory);
                    return amount;
                }
            }
        }
        OnInventoryUpdate?.Invoke(playerInventory);
        return amount;
    }
    public void ChangePlayerMoney(float amount) {
        playerMoney = Mathf.Max(playerMoney + amount,0);
        OnMoneyUpdate?.Invoke(playerMoney);
    }
}
