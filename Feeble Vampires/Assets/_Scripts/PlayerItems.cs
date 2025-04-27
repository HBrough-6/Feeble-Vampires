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

    [Header("Particle Systems")]
    public GameObject leechParticles;

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
            // use item 1
            if (equippedItemSlots[0])
            {
                UseItem(equippedItemNames[0]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // use item 2
            if (equippedItemSlots[1])
            {
                UseItem(equippedItemNames[1]);
            }
        }

        /*

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            
        }*/



        //if (Input.GetKeyDown(KeyCode.Keypad1)) itemSlotCheck("Broken Timepiece");
        //if (Input.GetKeyDown(KeyCode.Keypad2)) itemSlotCheck("Shriek");
        //if (Input.GetKeyDown(KeyCode.Keypad3)) itemSlotCheck("Mirage");
        //if (Input.GetKeyDown(KeyCode.Keypad4)) itemSlotCheck("Blood Dope");
        //if (Input.GetKeyDown(KeyCode.Keypad5)) itemSlotCheck("");
        //if (Input.GetKeyDown(KeyCode.Keypad6)) itemSlotCheck("Bat Buddy");
    }

    private void UseItem(string itemName)
    {
        switch (itemName)
        {
            case "Broken Timepiece":
                ActivateBrokenTimepiece();
                break;
            case "Shriek":
                ActivateShriek();
                break;
            case "Blood Dope":
                ActivateBloodDope();
                break;
            case "Leech":
                ActivateLeech();
                break;
            case "Bat Buddy":
                ActivateBatBuddy();
                break;

            default:
                break;
        }
    }

    public void ActivateLeech()
    {
        if (leech)
        {
            movementManager.gameManager.instakilled = true;
            movementManager.gameManager.gameOver();
            removeItem("Leech");

            leechParticles.SetActive(true);
        }
    }

    private void ActivateBatBuddy()
    {
        if (batBuddy && !movementManager.spawningBatBuddy)
        {
            movementManager.prepareBatBuddy();
            movementManager.spawningBatBuddy = true;
        }
    }

    private void ActivateBloodDope()
    {
        if (bloodDope && !movementManager.doping)
        {
            movementManager.dopeDouble();
            removeItem("Blood Dope");
        }
    }

    private void ActivateShriek()
    {
        if (shriek && !movementManager.isShrieking)
        {
            movementManager.startShrieking();
            removeItem("Shriek");
        }
    }

    private void ActivateBrokenTimepiece()
    {
        if (brokenTimePiece && !movementManager.timePieceActive)
        {
            movementManager.hyperExtendTime();
            movementManager.timePieceActive = true;
            removeItem("Broken Timepiece");
        }
    }

    public void itemSlotCheck(string itemName)
    {
        for (int i = 0; i < equippedItemSlots.Count; i++)
        {
            if (equippedItemNames[i] == itemName)
            {
                return;
            }
            else if (!equippedItemSlots[i])
            {
                //Debug.Log("here");
                equippedItemSlots[i] = true;
                equippedItemNames[i] = itemName;

                if (itemName == "Broken Timepiece")
                {
                    brokenTimePiece = true;
                }
                else if (itemName == "Shriek")
                {
                    shriek = true;
                }
                else if (itemName == "Mirage")
                {
                    mirage = true;
                }
                else if (itemName == "Blood Dope")
                {
                    bloodDope = true;
                }
                else if (itemName == "Leech")
                {
                    leech = true;
                }
                else if (itemName == "Bat Buddy")
                {
                    batBuddy = true;
                }

                return;
            }
        }
    }

    public void removeItem(string itemName)
    {
        for (int i = 0; i < equippedItemSlots.Count; i++)
        {
            if (equippedItemNames[i] == itemName && equippedItemSlots[i])
            {
                equippedItemNames[i] = "";
                equippedItemSlots[i] = false;

                if (itemName == "Broken Timepiece")
                {
                    brokenTimePiece = false;
                }
                else if (itemName == "Shriek")
                {
                    shriek = false;
                }
                else if (itemName == "Mirage")
                {
                    mirage = false;
                }
                else if (itemName == "Blood Dope")
                {
                    bloodDope = false;
                }
                else if (itemName == "Leech")
                {
                    leech = false;
                }
                else if (itemName == "Bat Buddy")
                {
                    batBuddy = false;
                }
            }
        }
    }
}
