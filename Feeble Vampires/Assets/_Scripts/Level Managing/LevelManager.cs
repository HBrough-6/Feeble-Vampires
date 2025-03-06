using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int SigilsRequiredForUnlock = 1;
    public int currentSigilsCollected = 0;

    private GridManager gridManager;
    private MovementManager movementManager;

    private Vector2Int startLocation;
    private Vector2Int[] doorLocations;

    private int currentLevel = 1;

    private void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        movementManager = FindAnyObjectByType<MovementManager>();
        doorLocations = new Vector2Int[2];
    }

    public void GenerateLevelOne()
    {
        // reset grid
        gridManager.GenerateGrid();
    }

    public void GenerateLevelTwo()
    {
        // reset the grid
        gridManager.GenerateGrid();
    }

    public void FailLevel()
    {
        // open the fail screen
    }

    private void CompleteLevel()
    {
        // increase level count
        currentLevel++;
        if (currentLevel == 2)
        {
            // pop up Skill select Screen
            gridManager.CreateGrid(2, 2);
            // gridManager.CreateGrid(2, 2);
        }
        else if (currentLevel >= 3)
        {
            // you win screen
        }
    }

    public void AttemptDoorOpen()
    {
        if (currentSigilsCollected >= SigilsRequiredForUnlock)
        {
            CompleteLevel();
            Debug.Log("Hi");
        }
    }

    public void PickUpSigil()
    {
        currentSigilsCollected++;
    }

    public void SetSigilRequirement(int sigils)
    {
        SigilsRequiredForUnlock = sigils;
        currentSigilsCollected = 0;
    }

    public void SetStartLocation(Vector2Int sPos)
    {
        startLocation = sPos;
        movementManager.setPlayerPos(gridManager.CellToWorldPos(startLocation));
        movementManager.playerPosInGrid = startLocation;
    }

    public void SetDoorLocation(Vector2Int dPos)
    {
        doorLocations[0] = dPos;
        doorLocations[1] = dPos + new Vector2Int(1, 0);
    }
}
