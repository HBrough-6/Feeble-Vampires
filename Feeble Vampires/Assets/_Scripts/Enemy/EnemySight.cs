using UnityEngine;

public class EnemySight : MonoBehaviour
{
    private Vector2Int[] seenTilesLocations;
    private bool[] tileSeen;

    private GridManager gridManager;
    private EnemyMovement enemyMovement;
    private EnemyBrain enemyBrain;

    private Vector2Int[] rightSight;
    private Vector2Int[] leftSight;
    private Vector2Int[] upSight;
    private Vector2Int[] downSight;

    private Transform SightTileParent;

    private GameObject[] sightTileObjects;
    public GameObject sightTilePrefab;

    private FakePlayer player;

    private void Awake()
    {

        gridManager = FindObjectOfType<GridManager>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyBrain = GetComponent<EnemyBrain>();

        player = FindObjectOfType<FakePlayer>();

        sightTileObjects = new GameObject[8];
        seenTilesLocations = new Vector2Int[8];
        tileSeen = new bool[8];

        rightSight = new Vector2Int[8];
        leftSight = new Vector2Int[8];
        upSight = new Vector2Int[8];
        downSight = new Vector2Int[8];

        #region Initializing SightTiles
        rightSight[0] = new Vector2Int(1, 0);
        rightSight[1] = new Vector2Int(2, 0);
        rightSight[2] = new Vector2Int(3, 1);
        rightSight[3] = new Vector2Int(4, 1);
        rightSight[4] = new Vector2Int(3, 0);
        rightSight[5] = new Vector2Int(4, 0);
        rightSight[6] = new Vector2Int(3, -1);
        rightSight[7] = new Vector2Int(4, -1);


        leftSight[0] = new Vector2Int(-1, 0);
        leftSight[1] = new Vector2Int(-2, 0);
        leftSight[2] = new Vector2Int(-3, 1);
        leftSight[3] = new Vector2Int(-4, 1);
        leftSight[4] = new Vector2Int(-3, 0);
        leftSight[5] = new Vector2Int(-4, 0);
        leftSight[6] = new Vector2Int(-3, -1);
        leftSight[7] = new Vector2Int(-4, -1);


        upSight[0] = new Vector2Int(0, 1);
        upSight[1] = new Vector2Int(0, 2);
        upSight[2] = new Vector2Int(-1, 3);
        upSight[3] = new Vector2Int(-1, 4);
        upSight[4] = new Vector2Int(0, 3);
        upSight[5] = new Vector2Int(0, 4);
        upSight[6] = new Vector2Int(1, 3);
        upSight[7] = new Vector2Int(1, 4);


        downSight[0] = new Vector2Int(0, -1);
        downSight[1] = new Vector2Int(0, -2);
        downSight[2] = new Vector2Int(-1, -3);
        downSight[3] = new Vector2Int(-1, -4);
        downSight[4] = new Vector2Int(0, -3);
        downSight[5] = new Vector2Int(0, -4);
        downSight[6] = new Vector2Int(1, -3);
        downSight[7] = new Vector2Int(1, -4);

        #endregion

        SightTileParent = transform.GetChild(1);

        // create sight tiles
        for (int i = 0; i < 8; i++)
        {
            GameObject temp = Instantiate(sightTilePrefab, SightTileParent.transform.position, transform.rotation, SightTileParent);
            sightTileObjects[i] = temp;
            temp.SetActive(false);
        }
    }

    public void DetermineSightline()
    {
        Vector2Int moveDir = enemyBrain.moveDir;
        Vector2Int[] tempTiles = new Vector2Int[8];
        Vector2Int[] sightedTileLocations = new Vector2Int[8];
        Vector2Int enemyPos = enemyBrain.posInGrid;

        ResetSightline();

        #region Determine Direction
        // facing right
        if (moveDir == new Vector2Int(1, 0))
        {
            tempTiles = rightSight;
        }
        // facing left
        else if (moveDir == new Vector2Int(-1, 0))
        {
            tempTiles = leftSight;

        }
        // facing forwards
        else if (moveDir == new Vector2Int(0, 1))
        {
            tempTiles = upSight;
        }
        // facing backwards
        else if (moveDir == new Vector2Int(0, -1))
        {
            tempTiles = downSight;
        }
        #endregion

        // assign each of the sight locations
        for (int i = 0; i < 8; i++)
        {
            sightedTileLocations[i] = enemyPos + tempTiles[i];
            sightTileObjects[i].transform.position = gridManager.CellToWorldPos(sightedTileLocations[i].x, sightedTileLocations[i].y);
        }

        // is the first tile obstructed
        if (CheckSightTileValid(sightedTileLocations[0]))
        {
            tileSeen[0] = true;
            // is the second tile obstructed
            if (CheckSightTileValid(sightedTileLocations[1]))
            {
                tileSeen[1] = true;
                // check if sighted tile location 2 and 3 are seen
                if (CheckSightTileValid(sightedTileLocations[2]))
                {
                    tileSeen[2] = true;
                    if (CheckSightTileValid(sightedTileLocations[3]))
                        tileSeen[3] = true;
                }
                // check if sighted tile location 4 and 5 are seen
                if (CheckSightTileValid(sightedTileLocations[4]))
                {
                    tileSeen[4] = true;
                    if (CheckSightTileValid(sightedTileLocations[5]))
                        tileSeen[5] = true;
                }
                // check if sighted tile location 6 and 7 are seen
                if (CheckSightTileValid(sightedTileLocations[6]))
                {
                    tileSeen[6] = true;
                    if (CheckSightTileValid(sightedTileLocations[7]))
                        tileSeen[7] = true;
                }

            }
        }


        CheckForPlayer(sightedTileLocations);
        DisplaySightline();
    }

    private void ResetSightline()
    {
        // reset the seen tiles
        for (int i = 0; i < 8; i++)
        {
            tileSeen[i] = false;
            sightTileObjects[i].SetActive(false);
        }
    }

    private void DisplaySightline()
    {
        for (int i = 0; i < 8; i++)
        {
            if (tileSeen[i])
            {
                sightTileObjects[i].SetActive(true);
            }
        }
    }

    private bool CheckSightTileValid(Vector2Int location)
    {
        return !gridManager.GetTileObstructed(location.x, location.y) &&
            location.x >= 0 && location.y >= 0 &&
            location.x < gridManager.width * 8 && location.y < gridManager.height * 8;
    }

    private void CheckForPlayer(Vector2Int[] seenTiles)
    {
        for (int i = 0; i < 8; i++)
        {
            if (tileSeen[i] && player != null)
            {
                if (player.posInGrid == seenTiles[i])
                {
                    player.seen();
                }
            }
        }
    }
}
