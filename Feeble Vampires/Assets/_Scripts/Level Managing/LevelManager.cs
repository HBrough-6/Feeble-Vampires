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
    private EnemyManager enemyManager;
    private UIManager uiManager;
    private GameManager gameManager;
    private PlayerAbilities playerAbilities;
    private Shop shop;

    private Vector2Int startLocation;
    private Vector2Int[] doorLocations;
    public GameObject winText;

    // public accessor for startLocation
    public Vector2Int StartLocation => startLocation;

    // The level that zone two starts at
    // public int zoneTwoStart = 6;

    public int levelsPerZoneEasy = 3;
    public int levelsPerZoneMedium = 4;
    public int levelsPerZoneHard = 5;

    public int currentLevelsPerZone;
    private int totalNumLevels;

    public Difficulty difficulty = Difficulty.Easy;

    public int currentLevel = 0;
    public int currentZone = 1;
    public bool safeZoneVisited = false;

    public bool inSafeZone = false;

    public SafeZone safeZone;

    private void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        movementManager = FindAnyObjectByType<MovementManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        doorLocations = new Vector2Int[2];

        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        playerAbilities = FindObjectOfType<PlayerAbilities>();
        shop = FindObjectOfType<Shop>();
    }

    private void Start()
    {
        /*gridManager.GenerateGrid();
        GenerateLevelOne();*/
        safeZone = FindObjectOfType<SafeZone>();
        currentLevel = 0;
        SetGameDifficulty(difficulty);
        GoToNextLevel();
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
                    case 1:
                    case 2:
                    case 3:
                        return new Vector2Int(2, 2);
                    case 4:
                    case 5:
                        return rand > 0.5 ? new Vector2Int(3, 2) : new Vector2Int(2, 3);
                    case 6:
                    case 7:
                        Debug.Log("last level easy");
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
                        Debug.Log("last level med");
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
                        Debug.Log("last level hard");
                        return rand > 0.5 ? new Vector2Int(5, 4) : new Vector2Int(4, 5);

                    default:
                        Debug.Log("Hard difficulty only has randomly generated layouts on levels 2-11");
                        return new Vector2Int(-1, -1);
                }
            default:
                Debug.Log("How did you get here, there are only 3 difficulty levels");
                return new Vector2Int(-1, -1);
        }
    }

    public void GoToNextLevel()
    {
        safeZone.DeactivateSafeZone();
        if (!inSafeZone && currentLevel != 0)
        {
            // give the player experience points
            //playerAbilities.GainXP(2);
        }
        enemyManager.ClearAllEnemies();


        inSafeZone = false;
        shop.visited = false;

        // Debug.Log(currentLevel);
        Vector2Int LevelSize = GenerateDifficulty(currentLevel);
        movementManager.UpdateHeightAndWidth(LevelSize.y, LevelSize.x);

        int rand = Random.Range(0, 10);
        // player is in zone 1 and the safe space hasn't been visited yet
        if (currentLevel == 0)
        {
            // import level 1;
            GenerateLevelOne();
            currentLevel++;
        }
        // the safe zone has not been visited - 4/10 chance to spawn the safe zone - formula to check if the level is the last in the zone
        else if (!safeZoneVisited && ((rand > 5) || currentLevel == currentLevelsPerZone * currentZone + 1))
        {
            //Debug.Log("!" + safeZoneVisited + rand + " > 5" + currentLevel + " == " + currentLevelsPerZone + " * " + currentZone + " + 1");
            // generate the floor
            gridManager.width = 2;
            gridManager.height = 2;
            gridManager.GenerateGrid(false);
            movementManager.UpdateHeightAndWidth(gridManager.height, gridManager.width);
            SetStartLocation(new Vector2Int(3, 0));

            // place the safe zone
            //Debug.Log("safeZone");
            safeZone.ActivateSafeZone();
            safeZoneVisited = true;

            SetSigilRequirement(0);

            gameManager.skillSelecting = true;

            inSafeZone = true;
            // pause the timer
            gameManager.skillSelecting = true;
        }
        // player is at the final level
        else if (currentLevel == totalNumLevels - 1)
        {
            Debug.Log("final level");
            // import final level
            // gridManager.StartFinalLevel()
            currentLevel++;
            winText.SetActive(true);
        }
        else if (currentLevel == totalNumLevels)
        {
            Debug.Log("You win");
            // end the game
            // pull up game winning screen
            winText.SetActive(true);
        }
        else
        {
            gameManager.skillSelecting = false;
            currentLevel++;
            Debug.Log("at level" + currentLevel);
            // reached the first level of zone 2, reset the safezonevisited
            if (currentLevel == currentLevelsPerZone + 2)
            {
                currentZone = 2;
                safeZoneVisited = false;
            }
            gridManager.width = LevelSize.x;
            gridManager.height = LevelSize.y;
            gridManager.GenerateGrid(true);
            gridManager.FillLevel(LevelSize.x, LevelSize.y);

            // spawn enemies
            enemyManager.SpawnEnemies(gridManager.sigilCount + 1);
        }

        uiManager.UpdateLevel(currentLevel);
    }

    public void AttemptDoorOpen()
    {
        if (currentSigilsCollected >= SigilsRequiredForUnlock)
        {
            FindObjectOfType<EnemyManager>().ClearAllEnemies();
            movementManager.endLevel();
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
        //Debug.Log("start position set to " + sPos);
        startLocation = sPos;
        movementManager.setPlayerPos(gridManager.CellToWorldPos(startLocation));
        movementManager.playerPosInGrid = startLocation;
    }

    public void SetDoorLocation(Vector2Int dPos)
    {
        doorLocations[0] = dPos;
        doorLocations[1] = dPos + new Vector2Int(1, 0);
    }

    public void SetDoorLocation(int x, int y)
    {
        doorLocations[0] = new Vector2Int(x, y);
        doorLocations[1] = new Vector2Int(x + 1, y);
    }
}
