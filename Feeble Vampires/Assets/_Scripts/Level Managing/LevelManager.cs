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

    public int currentLevel = 1;

    private void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        movementManager = FindAnyObjectByType<MovementManager>();
        doorLocations = new Vector2Int[2];
    }

    private void Start()
    {
        gridManager.GenerateGrid();
        GenerateLevelOne();
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

    private void CompleteLevel()
    {
        PlayerAbilities.experiencePoints += 2;
        // increase level count
        currentLevel++;
        if (currentLevel == 2)
        {
            // pop up Skill select Screen
            movementManager.endLevel();
        }
        else if (currentLevel >= 3)
        {
            // you win screen
            FindObjectOfType<GameManager>().Win();
            winText.SetActive(true);
        }
    }

    public void AttemptDoorOpen()
    {
        if (currentSigilsCollected >= SigilsRequiredForUnlock)
        {
            FindObjectOfType<EnemyManager>().ClearAllEnemies();
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
