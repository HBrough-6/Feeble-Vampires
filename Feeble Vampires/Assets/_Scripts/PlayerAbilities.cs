using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool canEcholocate;
    public bool isSwifter;
    public int swiftLevel;
    public bool canRushAttack;
    public bool smarter;
    public bool hideable;
    public bool isGreedy;

    public UIManager uiManager;
    public MovementManager movementManager;

    public int experiencePoints;

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

    public void GetSwift()
    {
        swiftLevel += 1;
        movementManager.spaceCap += swiftLevel;
        spendPoints();
    }

    public void GetSmart()
    {
        movementManager.timeLimit = movementManager.baseTime + 2;
        spendPoints();
    }

    public void spendPoints()
    {
        if (isGreedy) experiencePoints--;
        else experiencePoints -= 2;
    }
}
