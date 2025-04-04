using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    // be able to set a difficulty for the game
    // number of levels changes based on the difficulty

    public int SigilsRequiredForUnlock = 1;
    public int currentSigilsCollected = 0;

    private GridManager gridManager;
    private MovementManager movementManager;

    private Vector2Int startLocation;
    private Vector2Int[] doorLocations;
    public GameObject winText;

    // The level that zone two starts at
    // public int zoneTwoStart = 6;

    public int levelsPerZoneEasy = 3;
    public int levelsPerZoneMedium = 4;
    public int levelsPerZoneHard = 5;
    public int currentLevelsPerZone;
    private int totalNumLevels;

    public Difficulty difficulty = Difficulty.Easy;

    public int currentLevel = 1;
    public bool safeZoneOneVisited = false;
    public bool safeZoneTwoVisited = false;

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

    public void SetGameDifficulty(Difficulty difficulty)
    {
        this.difficulty = difficulty;
        switch (this.difficulty)
        {
            case Difficulty.Easy:
                currentLevelsPerZone = levelsPerZoneEasy;
                break;
            case Difficulty.Medium:
                currentLevelsPerZone = levelsPerZoneMedium;
                break;
            case Difficulty.Hard:
                currentLevelsPerZone = levelsPerZoneHard;
                break;
            default:
                break;
        }
        totalNumLevels = currentLevelsPerZone * 2 + 2;
        Debug.Log("Game difficulty set to " + this.difficulty + ". There are " + currentLevelsPerZone + " levels per zone and " + totalNumLevels + " levels in total");
    }

    private Vector2Int GenerateDifficulty(int currentLevel)
    {
        int rand = Random.Range(0, 2);
        switch (difficulty)
        {
            case Difficulty.Easy:
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
                    default:
                        Debug.Log("Easy difficulty difficulty only has randomly generated layouts on 2-7");
                        return new Vector2Int(-1, -1);

                }
            case Difficulty.Medium:
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
                        Debug.Log("Medium difficulty only has randomly generated layouts on levels 2-9");
                        return new Vector2Int(-1, -1);

                }
            case Difficulty.Hard:
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
                    case 10:
                    case 11:
                        return rand > 0.5 ? new Vector2Int(5, 4) : new Vector2Int(4, 5);

                    default:
                        Debug.Log("Hard difficulty only has randomly generated layouts on levels 2-11");
                        return new Vector2Int(-1, -1);
                }
                break;
            default:
                Debug.Log("How did you get here, there are only 3 difficulty levels");
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
        // check that the player hasn't visited the safe zone in its current zone
        else if ((!safeZoneOneVisited && currentLevel <= currentLevelsPerZone + 1) || (!safeZoneTwoVisited && currentLevel < totalNumLevels))
        {
            // attempt to visit safe zone
            if ((Random.Range(0, 10) + currentLevel % currentLevelsPerZone > 10 || currentLevel == currentLevelsPerZone + 1 || currentLevel == totalNumLevels - 1) && false)
            {
                // gridManager.VisitSafeZone();
                Debug.Log("Go to Safezone");
            }
            else
            {
                Debug.Log("daamk");
                gridManager.width = LevelSize.x;
                gridManager.height = LevelSize.y;
                gridManager.GenerateGrid();
                gridManager.FillLevel(LevelSize.x, LevelSize.y);
            }
        }
        else if (currentLevel == totalNumLevels - 1)
        {
            // import final level
            // gridManager.StartFinalLevel()
        }
        else if (currentLevel == totalNumLevels)
        {
            // end the game
            // pull up game winning screen
        }
        else
        {
            Debug.Log("dasfsffsdfamk");
            gridManager.width = LevelSize.x;
            gridManager.height = LevelSize.y;
            gridManager.GenerateGrid();
            gridManager.FillLevel(LevelSize.x, LevelSize.y);
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
