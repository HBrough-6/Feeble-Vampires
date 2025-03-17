using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool canEcholocate;
    public bool isSwifter;
    public bool canRushAttack;
    public bool smarter;
    public bool hideable;
    public bool isGreedy;
    public bool strongestInstinct;
    public bool scentTracker;

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
            movementManager.spaceCap += 1;
        }

        if (smarter)
        {
            movementManager.timeLimit = movementManager.baseTime + 2;
        }

        if (strongestInstinct)
        {
            movementManager.timeLimit /= 2;
            movementManager.spaceCap *= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {

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
