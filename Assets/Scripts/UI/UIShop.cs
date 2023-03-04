using TMPro;
using UnityEngine;

public class UIShop : MonoBehaviour{
    [SerializeField] private GameObject shopPrefab;
    [SerializeField] private GameObject scrollRefrence;
    [SerializeField] private TextMeshProUGUI shopKeepMoney;
    private void Start() {
        ShopManager.Instance.CurrentShop.OnMoneyUpdate += CurrentShop_OnMoneyUpdate;
        ShopManager.Instance.CurrentShop.OnInventoryUpdate += CurrentShop_OnInventoryUpdate;
    }

    private void OnDestroy() {
        ShopManager.Instance.CurrentShop.OnMoneyUpdate -= CurrentShop_OnMoneyUpdate;
        ShopManager.Instance.CurrentShop.OnInventoryUpdate -= CurrentShop_OnInventoryUpdate;
    }

    private void CurrentShop_OnInventoryUpdate(System.Collections.Generic.List<ItemStackableObject> obj) {
        Debug.Log($"Attempting to update the shop inventory {obj.Count}");
        foreach (Transform item in scrollRefrence.transform) {
            GameObject.Destroy(item.gameObject);
        }
        foreach (ItemStackableObject item in obj) {
            GameObject shopButton = Instantiate(shopPrefab);
            shopButton.transform.SetParent(scrollRefrence.transform);
            StorePrefab buttonContainers = shopButton.GetComponent<StorePrefab>();
            buttonContainers.itemName.text = $"Name:{item.Item.itemName}";
            buttonContainers.itemCost.text = $"Cost:{item.Item.itemCost}";
            buttonContainers.itemSell.text = $"ReSell:{item.Item.itemSell}";
            buttonContainers.itemAmount.text = $"Amount:{item.ItemAmount}";
            buttonContainers.Buy.onClick.AddListener(() => { ShopManager.Instance.BuyItem(item.Item, 1); }) ;
        }
    }

    private void CurrentShop_OnMoneyUpdate(float obj) {
        Debug.Log($"Attempting to update the shop money {obj}");
        shopKeepMoney.text = $"Money:{obj}";
    }
}
