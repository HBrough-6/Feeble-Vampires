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

    public static int experiencePoints;

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

        experiencePoints = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetSwift()
    {
        if (experiencePoints < 2)
        {
            Debug.Log("You do not have enough experience points");
        }
        else
        {
            spendPoints();
            swiftLevel += 1;
            movementManager.spaceCap += swiftLevel;
        }
            
    }

    public void GetSmart()
    {
        if (experiencePoints < 2)
        {
            Debug.Log("You do not have enough experience points");
        }
        else
        {
            spendPoints();
            movementManager.timeLimit = movementManager.baseTime + 2;
        }
    }

    public void spendPoints()
    {
        if (isGreedy) experiencePoints--;
        else experiencePoints -= 2;   
    }
}
