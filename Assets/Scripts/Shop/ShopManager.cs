using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour{
    #region Private.
    [SerializeField] private List<ItemScriptableObject> shopItems = new List<ItemScriptableObject>();
    private Shop currentShop = new Shop();
    #endregion
    #region Getters Setters.
    public static ShopManager Instance { get; private set; }
    public List<ItemScriptableObject> ShopItems { get => shopItems; }
    public Shop CurrentShop { get => currentShop; }
    #endregion


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            NextShop();
        }
    }

    #region Methods.
    private void NextShop() {
        Debug.Log("Calling for next shop");
        currentShop.GenerateShop();
    }
    public void BuyItem(ItemScriptableObject item, int amount) {
        currentShop.BuyItem(item,amount);
    }  
    public void SellItem(ItemScriptableObject item, int amount) {
        currentShop.SellItem(item, amount);
    }
    #endregion
}
