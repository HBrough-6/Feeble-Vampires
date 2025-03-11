using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public MovementManager movementManager;

    public bool brokenTimePiece;

    public List<bool> itemSlots;

    // Start is called before the first frame update
    void Start()
    {
        movementManager = FindObjectOfType<MovementManager>();
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
    }
}
