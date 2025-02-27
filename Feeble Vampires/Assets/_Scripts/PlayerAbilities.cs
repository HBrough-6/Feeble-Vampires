using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool canEcholocate;
    public bool isSwifter;
    public int swiftLevel;
    public bool canRushAttack;
    public bool smarter;

    public UIManager uiManager;
    public MovementManager movementManager;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        movementManager = FindObjectOfType<MovementManager>();

        if (canEcholocate) uiManager.makeMap();

        if (isSwifter)
        {
            swiftLevel += 1;
        }

        if (smarter)
        {
            movementManager.timeLimit = movementManager.baseTime + 2;
        }

        movementManager.spaceCap += swiftLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
