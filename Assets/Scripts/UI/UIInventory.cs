using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject scrollRefrence;
    [SerializeField] private TextMeshProUGUI playerMoney;
    private void Start() {
        Player.Instance.OnMoneyUpdate += Instance_OnMoneyUpdate;
        Player.Instance.OnInventoryUpdate += Instance_OnInventoryUpdate;
    }


    private void OnDestroy() {
        Player.Instance.OnMoneyUpdate -= Instance_OnMoneyUpdate;
        Player.Instance.OnInventoryUpdate -= Instance_OnInventoryUpdate;
    }

    private void Instance_OnInventoryUpdate(System.Collections.Generic.List<ItemStackableObject> obj) {
        Debug.Log($"Attempting to update the player inventory {obj.Count}");
        foreach (Transform item in scrollRefrence.transform) {
            GameObject.Destroy(item.gameObject);
        }
        foreach (ItemStackableObject item in obj) {
            GameObject playerButton = Instantiate(playerPrefab);
            playerButton.transform.SetParent(scrollRefrence.transform);
            PlayerPrefab buttonContainers = playerButton.GetComponent<PlayerPrefab>();
            buttonContainers.itemName.text = $"Name:{item.Item.itemName}";
            buttonContainers.itemCost.text = $"Cost:{item.Item.itemCost}";
            buttonContainers.itemSell.text = $"ReSell:{item.Item.itemSell}";
            buttonContainers.itemAmount.text = $"Amount:{item.ItemAmount}";
            buttonContainers.Sell.onClick.AddListener(() => { ShopManager.Instance.SellItem(item.Item, 1); });
        }
    }

    private void Instance_OnMoneyUpdate(float obj) {
        Debug.Log($"Attempting to update the player money {obj}");
        playerMoney.text = $"Money:{obj}";
    }
}
