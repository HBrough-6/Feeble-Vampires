using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public MovementManager movementManager;

    public bool brokenTimePiece;
    public bool shriek;
    public bool mirage;
    public bool bloodDope;
    public bool batBuddy;
    public bool leech;

    public List<bool> equippedItemSlots;
    public List<string> equippedItemNames;

    // Start is called before the first frame update
    void Start()
    {
        movementManager = FindObjectOfType<MovementManager>();

        if (mirage)
        {
            movementManager.canSidestep = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (brokenTimePiece && !movementManager.timePieceActive)
            {
                movementManager.hyperExtendTime();
                movementManager.timePieceActive = true;
                removeItem(ref brokenTimePiece, "Broken Timepiece");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (shriek && !movementManager.isShrieking)
            {
                movementManager.startShrieking();
                removeItem(ref shriek, "Shriek");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (bloodDope && !movementManager.doping)
            {
                movementManager.dopeDouble();
                removeItem(ref bloodDope, "Blood Dope");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (batBuddy && !movementManager.spawningBatBuddy)
            {
                movementManager.prepareBatBuddy();
                movementManager.spawningBatBuddy = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (leech)
            {
                movementManager.gameManager.instakilled = true;
                movementManager.gameManager.gameOver();
            }
        }

        if (Input.GetKeyDown(KeyCode.Keypad1)) itemSlotCheck(ref brokenTimePiece, "Broken Timepiece");
        if (Input.GetKeyDown(KeyCode.Keypad2)) itemSlotCheck(ref shriek, "Shriek");
        if (Input.GetKeyDown(KeyCode.Keypad3)) itemSlotCheck(ref mirage, "Mirage");
        if (Input.GetKeyDown(KeyCode.Keypad4)) itemSlotCheck(ref bloodDope, "Blood Dope");
        //if (Input.GetKeyDown(KeyCode.Keypad5)) itemSlotCheck(ref leech, "Leech");
    }

    public void itemSlotCheck(ref bool newItem, string itemName)
    {
        for (int i = 0; i < equippedItemSlots.Count; i++)
        {
            if (equippedItemNames[i] == newItem.ToString() || newItem)
            {
                return;
            }
            else if (!equippedItemSlots[i])
            {
                equippedItemSlots[i] = true;
                equippedItemNames[i] = itemName;
                newItem = true;
                return;
            }
        }
    }

    public void removeItem(ref bool usedItem, string itemName)
    {
        for (int i = 0; i < equippedItemSlots.Count; i++)
        {
            if (equippedItemNames[i] == itemName)
            {
                equippedItemNames[i] = "";
                equippedItemSlots[i] = false;
                usedItem = false;
            }
        }
    }
}
