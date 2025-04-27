using UnityEngine;

// manages interactions in the safe zone
public class SafeZone : MonoBehaviour
{
    private GameObject zone;
    private TurnManager turnManager;
    private GameManager gameManger;

    private void Awake()
    {
        zone = transform.GetChild(0).gameObject;
        turnManager = FindObjectOfType<TurnManager>();
        gameManger = FindObjectOfType<GameManager>();
    }

    public void ActivateSafeZone()
    {
        zone.SetActive(true);
        gameManger.skillSelecting = true;
        RandomizeShopInventory();
        // turnManager.DisableTimer();
    }

    public void DeactivateSafeZone()
    {
        // turn off the gameobject
        zone.SetActive(false);
        // do something to the shopkeep?
        // turnManager.EnableTimer
    }
    private void RandomizeShopInventory()
    {
        // choose random upgrades
        // activate UI
    }

    // do these in the shopkeeper script
    public void OpenShop()
    {

    }

    public void CloseShop()
    {

    }
}
