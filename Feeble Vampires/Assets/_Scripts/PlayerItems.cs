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

    public List<bool> equippedItemSlots;

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
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (shriek && !movementManager.isShrieking)
            {
                movementManager.startShrieking();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (bloodDope && !movementManager.doping)
            {
                movementManager.dopeDouble();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (batBuddy && !movementManager.spawningBatBuddy)
            {
                movementManager.prepareBatBuddy();
                movementManager.spawningBatBuddy = true;
            }
        }
    }
}
