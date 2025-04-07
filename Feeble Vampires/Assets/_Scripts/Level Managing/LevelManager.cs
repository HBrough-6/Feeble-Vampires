using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int SigilsRequiredForUnlock = 1;
    public int currentSigilsCollected = 0;

    private GridManager gridManager;
    private MovementManager movementManager;

    private Vector2Int startLocation;
    private Vector2Int[] doorLocations;
    public GameObject winText;

    // The level that zone two starts at
    public int zoneTwoStart = 6;

    public int currentLevel = 1;
    public bool safeZoneVisited = false;

    private void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        movementManager = FindAnyObjectByType<MovementManager>();
        doorLocations = new Vector2Int[2];
    }

    private void Start()
    {
        /*gridManager.GenerateGrid();
        GenerateLevelOne();*/
    }

    public void GenerateLevelOne()
    {
        //load level one
        gridManager.FakeLevelOne();
    }

    public void GenerateLevelTwo()
    {
        // reset the grid
        gridManager.FakeLevelTwo();
    }

    public void FailLevel()
    {
        // open the fail screen
    }

    private Vector2Int GenerateDifficulty(int currentLevel)
    {
        int rand = Random.Range(0, 2);
        // levels 2 and 3 are 2x2
        // levels 4 and 5 are 2x3 or 3x2
        // levels 6 and 7 are 3x3
        // levels 8 and 9 are 4x3 or 3x4
        switch (currentLevel)
        {
            case 2:
            case 3:
                return new Vector2Int(2, 2);
            case 4:
            case 5:
                return rand > 0.5 ? new Vector2Int(3, 2) : new Vector2Int(2, 3);
            case 6:
            case 7:
                return new Vector2Int(3, 3);

            case 8:
            case 9:
                return rand > 0.5 ? new Vector2Int(3, 4) : new Vector2Int(4, 3);


            default:
                Debug.Log("Only generates Difficulty for levels 2-9");
                return new Vector2Int(-1, -1);

        }
    }

    public void GoToNextLevel()
    {
        // increase current level count
        currentLevel++;
        Debug.Log(currentLevel);
        Vector2Int LevelSize = GenerateDifficulty(currentLevel);

        // player is in zone 1 and the safe space hasn't been visited yet
        if (currentLevel == 1)
        {
            // import level 1;
            GenerateLevelOne();
        }
        else if (!safeZoneVisited)
        {
            //// attempt to visit safe zone
            //if (Random.Range(0, 10) + currentLevel % zoneTwoStart > 9 || currentLevel == 5 || currentLevel == 9 && false)
            //{
            //    // gridManager.VisitSafeZone();
            //    Debug.Log("Go to Safezone");
            //}
            //else
            //{
            //    gridManager.width = LevelSize.x;
            //    gridManager.height = LevelSize.y;
            //    gridManager.GenerateGrid();
            //    gridManager.FillLevel(LevelSize.x, LevelSize.y);
            //}

            gridManager.width = LevelSize.x;
            gridManager.height = LevelSize.y;
            gridManager.GenerateGrid();
            gridManager.FillLevel(LevelSize.x, LevelSize.y);
        }
        else if (currentLevel == 9)
        {
            // import final level
            // gridManager.StartLevelTen()
        }
        else if (currentLevel == 10)
        {
            // end the game

        }

    }

    public void AttemptDoorOpen()
    {
        if (currentSigilsCollected >= SigilsRequiredForUnlock)
        {
            FindObjectOfType<EnemyManager>().ClearAllEnemies();
            movementManager.endLevel();
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
