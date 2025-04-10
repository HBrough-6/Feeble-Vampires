using UnityEngine;

public class Shop : MonoBehaviour
{
    private ItemSO[] availableItems = new ItemSO[3];

    public void SetUp()
    {
        ChooseRandomItems();
        UpdateShopUI();
    }

    public void OpenShop()
    {
        // uiManager.EnableShopWindow();
    }

    public void CloseShop()
    {
        // uiManager.DisableShopWindow();
    }

    public void ChooseRandomItems()
    {
        // get list of items from something
        // choose 2 or 3 (unsure how many) items
        // assign them to the current Items
    }

    public void UpdateShopUI()
    {
        // get scriptable objects with the info of items
        // assign stuff in the UI
        // UIManager.AssignItemInfo(0, ScriptableObject)
    }

    public void PurchaseItem(int itemIndex)
    {
        ItemSO pItem = availableItems[itemIndex];

    }
}
