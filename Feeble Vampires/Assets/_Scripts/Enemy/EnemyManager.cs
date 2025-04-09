using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GridManager gridManager;
    public GameManager gameManager;
    private MovementManager movementManager;
    private LevelManager levelManager;

    private List<EnemyBrain> enemies;
    private List<EnemyBrain> deadEnemies;

    public List<Vector2Int> enemyStartPoints = new List<Vector2Int>();

    public int MinPathDist = 7;

    // displays enemy paths when created
    public bool displayPath = true;

    [SerializeField] private GameObject EnemyPrefab;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        gameManager = FindObjectOfType<GameManager>();
        movementManager = FindObjectOfType<MovementManager>();
        levelManager = FindObjectOfType<LevelManager>();

        enemies = new List<EnemyBrain>();
        deadEnemies = new List<EnemyBrain>();
    }

    /// <summary>
    /// Add an enemy to the list of living enemies
    /// </summary>
    /// <param name="enemy">enemy to add</param>
    public void AddEnemy(EnemyBrain enemy)
    {
        enemies.Add(enemy);
    }

    public void ClearAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyDied(enemies[i]);
            i--;
        }
        while (deadEnemies.Count > 0)
        {
            Destroy(deadEnemies[0].gameObject);
            deadEnemies.RemoveAt(0);
        }
        enemyStartPoints = new List<Vector2Int>();
    }

    /// <summary>
    /// Removes enemy from the list of living enemies and sets it inactive
    /// </summary>
    /// <param name="deadEnemy">The enemy that died</param>
    public void EnemyDied(EnemyBrain deadEnemy)
    {
        deadEnemies.Add(deadEnemy);
        enemies.Remove(deadEnemy);
        deadEnemy.gameObject.SetActive(false);
    }

    /// <summary>
    /// Causes all enemies to take a turn
    /// </summary>
    public void EnemiesTakeTurn()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!movementManager.isShrieking) enemies[i].Activate();
        }
    }

    public void PlayerSpotted()
    {
        gameManager.gameOver();
    }

    public void CreateEnemy()
    {
        // create the enemy instance
        EnemyMovement movement = Instantiate(EnemyPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<EnemyMovement>();

        // find where the player starts and avoid starting there
        Vector2Int startLocation = levelManager.StartLocation;
        List<Vector2Int> chunks = new List<Vector2Int>();

        // list to store the path the enemy will take
        List<Vector2Int> path = new List<Vector2Int>();

        // get the size of the grid
        int width = gridManager.width;
        int height = gridManager.height;
        // create a list with chunks to choose from
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x != startLocation.x && y != startLocation.y)
                {
                    chunks.Add(new Vector2Int(x, y));
                }
            }
        }

        // find all nodes that are not a wall
        List<Vector2Int> openTiles = new List<Vector2Int>();
        DTile[] dGrid = gridManager.dGrid.grid;
        for (int i = 0; i < gridManager.dGrid.grid.Length; i++)
        {
            if (dGrid[i].type != 1 && dGrid[i].type != 2)
            {
                openTiles.Add(dGrid[i].pos);
            }
        }

        int timesRun = 0;
        // find a random starting point that is not in the startingChunk and that are at least 7 blocks away
        Vector2Int enemyStartPos = openTiles[UnityEngine.Random.Range(0, openTiles.Count)];
        while (gridManager.GetChunkLocation(enemyStartPos) == gridManager.GetChunkLocation(startLocation)
            || Vector2Int.Distance(enemyStartPos, startLocation) < 7
            || IsDuplicateStartPoint(enemyStartPos))
        {
            enemyStartPos = openTiles[UnityEngine.Random.Range(0, openTiles.Count)];
            timesRun++;
        }
        //Debug.Log("enemy chunk " + gridManager.GetChunkLocation(enemyStartPos) + " player chunk: " + gridManager.GetChunkLocation(startLocation));
        // Debug.Log("Found new enemy starting position " + timesRun + " times");

        // choose first destination
        timesRun = 0;
        // find a random ending point that is not in the startingChunk and that are at least 6 blocks away
        // and make sure the new destination is at least 5 tiles away from the enemy's starting position
        Vector2Int firstDestination = openTiles[UnityEngine.Random.Range(0, openTiles.Count)];
        while (gridManager.GetChunkLocation(firstDestination) == gridManager.GetChunkLocation(startLocation)
            || Vector2Int.Distance(firstDestination, startLocation) < 5
            || Vector2Int.Distance(firstDestination, enemyStartPos) < MinPathDist
            || startLocation == firstDestination)
        {
            firstDestination = openTiles[UnityEngine.Random.Range(0, openTiles.Count)];
            timesRun++;
        }
        // Debug.Log("Start at: " + enemyStartPos + " end at: " + firstDestination);
        movement.SetPosInGrid(enemyStartPos);
        path = (movement.CreatePathToPoint(firstDestination));


        // pathfind 3-4 nodes, can have one of the later nodes in the starting player chunk
        movement.moveNodes = path.ToArray();

        if (displayPath)
        {
            // display the paths of the enemy
            for (int i = 0; i < path.Count; i++)
            {
                Instantiate(gridManager.foundTilePrefab, gridManager.CellToWorldPos(path[i]), Quaternion.Euler(0, 0, 0));
            }
        }
        AddEnemy(movement.GetComponent<EnemyBrain>());
    }

    // returns true if the pass in position is already an enemy start point
    private bool IsDuplicateStartPoint(Vector2Int pos)
    {
        for (int i = 0; i < enemyStartPoints.Count; i++)
        {
            if (pos == enemyStartPoints[i])
            {
                // Debug.Log("found duplicate");
                return true;
            }
        }
        return false;
    }

    public void SpawnEnemies(int numEnemies)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            CreateEnemy();
            // make sure that the enemy has at least 2 nodes to walk to
            // probably not the best way to do it, should be integrated into the path checking later
            while (enemies[i].GetComponent<EnemyMovement>().moveNodes.Length < 2)
            {
                // Debug.Log("enemy had only 1 move node");
                Destroy(enemies[i].gameObject);
                enemies.RemoveAt(i);
                CreateEnemy();
            }
        }
    }
}
