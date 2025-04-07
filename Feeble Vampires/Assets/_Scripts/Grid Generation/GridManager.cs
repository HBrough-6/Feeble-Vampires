using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Array2DEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Goal: Use a modified version of BFS to find out which tiles are discoverable from a random point in the bottom row.
    // find each spot that has a sigil, count how many accessible tiles there are, spawn a door

    // DONE store the best starting point's found tiles outside of the loop
    // DONE don't spawn sigils when they are found in chunks, add their positions to a list
    // DONE don't store the starting positions, just check if they are better
    // after finding the best starting position, check the sigil positions against the found tiles and spawn random ones
    // Choose a random sigil that is not in the same chunk as the start tile,
    // then choose sigils that are a minimum distance away from the original 
    // find a 2x1 area at the top of the map for doors to spawn

    public float timeBetweenIncrement = 0.2f;

    public LevelManager levelManager;

    public Chunk tempChunkStorage;
    public Grid grid;

    public int height;
    public int width;

    public int chunkSize = 8;

    public GameObject whiteTile;
    public GameObject blackTile;

    public GameObject wallPrefab;
    public GameObject obstructionPrefab;
    public GameObject sigilPrefab;
    public GameObject doorPrefab;
    public GameObject EnemyPrefab;

    public GameObject foundTilePrefab;

    public Transform obstructionsParent;
    public Transform tilesParent;
    public Transform SigilParent;

    public int obstructedTiles;

    private Chunk[] premadeChunks;
    private int premadeChunksCount;

    public List<Vector2Int> sigilLocations;

    public bool RotateTiles = true;

    public bool successfullyGenerated = false;

    private StartTileInfo tileInfo1;

    private int redoCount = 0;

    public DigitalGrid dGrid;


    public int[] resultingGrid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
        levelManager = FindObjectOfType<LevelManager>();
        sigilLocations = new List<Vector2Int>();
        premadeChunks = Resources.LoadAll<Chunk>("PreMade Chunks");
        premadeChunksCount = premadeChunks.Length;
    }

    private void Start()
    {
        GenerateGrid();
    }

    public IEnumerator CheckAllTiles()
    {
        for (int row = 0; row < height * 8; row++)
        {
            for (int col = 0; col < width * 8; col++)
            {

                if (!GetTileObstructed(col, row))
                {
                    Vector3 temp = CellToWorldPos(col, row);
                    Instantiate(foundTilePrefab, temp + Vector3.up + Vector3.up, transform.rotation, SigilParent);
                    yield return new WaitForSeconds(0.01f);
                }

            }
        }
    }

    public void CreateGrid(int wid, int hei)
    {
        width = wid;
        height = hei;

        GenerateGrid();
        //        FakeLevelTwo();

        StartTileInfo tileInfo = new StartTileInfo();


        bool success = false;

        // using for loop for infinite loop protection
        for (int i = 0; i < 200 && !success; i++)
        {
            Debug.Log("Generating new map");
            FillWholeGrid();
            //VerifyGrid(/*out tileInfo*/);
            StartCoroutine(VerifyGrid());
            //*yield return new WaitForSeconds(timeBetweenIncrement);*//*
        }

        if (success)
        {
            Debug.Log("Successfully generated");
            PlaceObjects(tileInfo);

        }
        else
        {
            Debug.Log("Failed Generation");
        }
    }

    #region GridCreation

    #region Build Levels
    public void FakeLevelTwo()
    {
        for (int i = obstructionsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(obstructionsParent.GetChild(i).gameObject);
        }
        for (int i = SigilParent.childCount - 1; i >= 0; i--)
        {
            Destroy(SigilParent.GetChild(i).gameObject);
        }


        // import 4 different preselected chunks
        ImportGridChunk(new Vector2Int(0, 0), Resources.Load("PreMade Chunks/GridChunk2") as Chunk, 3);
        ImportGridChunk(new Vector2Int(1, 0), Resources.Load("PreMade Chunks/GridChunk16") as Chunk, 0);
        ImportGridChunk(new Vector2Int(0, 1), Resources.Load("PreMade Chunks/GridChunk12") as Chunk, 0);
        ImportGridChunk(new Vector2Int(1, 1), Resources.Load("PreMade Chunks/GridChunk6") as Chunk, 0);

        Instantiate(sigilPrefab, CellToWorldPos(new Vector2Int(13, 10)), transform.rotation, obstructionsParent);
        Instantiate(sigilPrefab, CellToWorldPos(new Vector2Int(1, 14)), transform.rotation, obstructionsParent);
        Instantiate(doorPrefab, CellToWorldPos(new Vector2Int(13, 15)), transform.rotation, obstructionsParent);

        // 9,0 - 9,3 - 14,3 - 14,10 - pingpong move
        Vector2Int[] enemyOneMovement = { new Vector2Int(9, 0), new Vector2Int(9, 3), new Vector2Int(14, 3)
                , new Vector2Int(14, 10) };
        // 7,4 - 7,9 - 8,9 - 8,14 - 4,14 - 4,9 - 5,9, 5,4 - loop move
        Vector2Int[] enemyTwoMovement = { new Vector2Int(7, 4), new Vector2Int(7, 9), new Vector2Int(8, 9), new Vector2Int(8, 14)
        , new Vector2Int(4, 14), new Vector2Int(4, 9), new Vector2Int(5,9), new Vector2Int(5,4)};
        // 3,10 - 0,10 - 0,0 - 3,0 - 3,1 - pingpong
        Vector2Int[] enemyThreeMovement = { new Vector2Int(3, 10), new Vector2Int(0, 10)
                , new Vector2Int(0, 0), new Vector2Int(3, 0), new Vector2Int(3, 1) };

        /*EnemyMovement EnemyOne = Instantiate(EnemyPrefab, CellToWorldPos(new Vector2Int(9, 0)), transform.rotation).GetComponent<EnemyMovement>();
        EnemyOne.moveNodes = enemyOneMovement;
        EnemyOne.moveType = EnemyMovement.MoveType.PingPong;

        EnemyMovement EnemyTwo = Instantiate(EnemyPrefab, CellToWorldPos(new Vector2Int(7, 4)), transform.rotation).GetComponent<EnemyMovement>();
        EnemyTwo.moveNodes = enemyTwoMovement;
        EnemyTwo.moveType = EnemyMovement.MoveType.Loop;

        EnemyMovement EnemyThree = Instantiate(EnemyPrefab, CellToWorldPos(new Vector2Int(3, 10)), transform.rotation).GetComponent<EnemyMovement>();
        EnemyThree.moveNodes = enemyThreeMovement;
        EnemyThree.moveType = EnemyMovement.MoveType.PingPong;*/

        levelManager.SetStartLocation(new Vector2Int(1, 0));
        levelManager.SetSigilRequirement(2);
    }

    public void FakeLevelOne()
    {
        // import 4 different preselected chunks
        ImportGridChunk(new Vector2Int(0, 0), Resources.Load("PreMade Chunks/GridChunk10") as Chunk, 1);
        ImportGridChunk(new Vector2Int(1, 0), Resources.Load("PreMade Chunks/GridChunk6") as Chunk, 2);
        ImportGridChunk(new Vector2Int(0, 1), Resources.Load("PreMade Chunks/GridChunk15") as Chunk, 1);
        ImportGridChunk(new Vector2Int(1, 1), Resources.Load("PreMade Chunks/GridChunk9") as Chunk, 1);

        Instantiate(sigilPrefab, CellToWorldPos(new Vector2Int(11, 15)), transform.rotation, obstructionsParent);
        Instantiate(sigilPrefab, CellToWorldPos(new Vector2Int(11, 4)), transform.rotation, obstructionsParent);
        Instantiate(doorPrefab, CellToWorldPos(new Vector2Int(6, 15)), transform.rotation, obstructionsParent);

        // 9,0 - 9,3 - 14,3 - 14,10 - pingpong move
        Vector2Int[] enemyOneMovement = { new Vector2Int(8, 6), new Vector2Int(2, 6), new Vector2Int(2, 12) };
        // 7,4 - 7,9 - 8,9 - 8,14 - 4,14 - 4,9 - 5,9, 5,4 - loop move
        Vector2Int[] enemyTwoMovement = { new Vector2Int(7, 5), new Vector2Int(10, 5), new Vector2Int(10, 11) };
        // 3,10 - 0,10 - 0,0 - 3,0 - 3,1 - pingpong
        Vector2Int[] enemyThreeMovement = { new Vector2Int(15, 11), new Vector2Int(15, 0)
                , new Vector2Int(14, 0), new Vector2Int(14, 11)};

        /*EnemyMovement EnemyOne = Instantiate(EnemyPrefab, CellToWorldPos(new Vector2Int(9, 0)), transform.rotation).GetComponent<EnemyMovement>();
        EnemyOne.moveNodes = enemyOneMovement;
        EnemyOne.moveType = EnemyMovement.MoveType.PingPong;

        EnemyMovement EnemyTwo = Instantiate(EnemyPrefab, CellToWorldPos(new Vector2Int(7, 4)), transform.rotation).GetComponent<EnemyMovement>();
        EnemyTwo.moveNodes = enemyTwoMovement;
        EnemyTwo.moveType = EnemyMovement.MoveType.PingPong;

        EnemyMovement EnemyThree = Instantiate(EnemyPrefab, CellToWorldPos(new Vector2Int(3, 10)), transform.rotation).GetComponent<EnemyMovement>();
        EnemyThree.moveNodes = enemyThreeMovement;
        EnemyThree.moveType = EnemyMovement.MoveType.Loop;
*/
        levelManager.SetStartLocation(new Vector2Int(7, 0));
        levelManager.SetSigilRequirement(2);
    }

    #endregion

    public void GenerateGrid()
    {
        int cellHeight = height * chunkSize;
        int cellWidth = width * chunkSize;

        // clear tiles from the grid
        for (int i = tilesParent.childCount - 1; i >= 0; i--)
        {
            Destroy(tilesParent.GetChild(i).gameObject);
        }
        for (int i = obstructionsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(obstructionsParent.GetChild(i).gameObject);
        }
        for (int i = SigilParent.childCount - 1; i >= 0; i--)
        {
            Destroy(SigilParent.GetChild(i).gameObject);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        // possible point of failure
        // reset the sigil locations
        List<Vector2Int> sigilLocations = new List<Vector2Int>();

        // start at the grid Manager Position
        Vector3 cellPos = transform.position;
        Vector3Int cellPosInGrid = Vector3Int.zero;

        // delete old tiles


        bool isWhite = true;

        // spawn tiles at positions on grid
        for (int z = 0; z < cellHeight; z++)
        {
            isWhite = !isWhite;
            for (int x = 0; x < cellWidth; x++)
            {
                isWhite = !isWhite;

                cellPosInGrid.z = z;
                cellPosInGrid.x = x;
                cellPosInGrid.y = 0;

                Vector3 worldPos = grid.CellToWorld(cellPosInGrid);

                if (isWhite)
                {
                    Instantiate(whiteTile, worldPos, transform.rotation, tilesParent);
                }
                else
                {
                    Instantiate(blackTile, worldPos, transform.rotation, tilesParent);
                }
            }
        }
    }

    private void FillWholeGrid()
    {
        // reset the number of obstructed tiles
        obstructedTiles = 0;

        // clear tiles from the grid
        for (int i = obstructionsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(obstructionsParent.GetChild(i).gameObject);
        }
        for (int i = SigilParent.childCount - 1; i >= 0; i--)
        {
            Destroy(SigilParent.GetChild(i).gameObject);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        // possible point of failure
        // clear sigil locations
        sigilLocations.Clear();

        for (int chunkRow = 0; chunkRow < height; chunkRow++)
        {
            for (int chunkCol = 0; chunkCol < width; chunkCol++)
            {
                ImportGridChunk(new Vector2Int(chunkCol, chunkRow), GetChunkFromFiles());
            }
        }
    }

    // [0,1][1,1]
    // [0,0][1,0]
    public void ImportGridChunk(Vector2Int chunkPos, Chunk newChunk)
    {
        // get the starting positions
        int startX = chunkPos.x * 8;
        int startY = chunkPos.y * 8;

        Array2DInt newChunkGrid = RotateChunk(newChunk);

        // create variable for storing the tile position in the grid.
        Vector3Int tilePos = Vector3Int.zero;

        // iterate through the specified chunk
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                // check if the current piece is an obstructed tile
                int tileInfo = newChunkGrid.GetCell(col, row);

                // if the tile isn't empty, do something
                if (tileInfo != 0)
                {
                    // find the position of the tile
                    tilePos.x = startX + col;
                    tilePos.z = startY + row;
                    Vector3 obstructionPos = grid.CellToWorld(tilePos);
                    switch (tileInfo)
                    {
                        case 1:
                            // place a wall
                            Instantiate(wallPrefab, obstructionPos, transform.rotation, obstructionsParent);
                            obstructedTiles++;
                            break;

                        case 2:
                            // place an obstruction
                            Instantiate(obstructionPrefab, obstructionPos, transform.rotation, obstructionsParent);
                            obstructedTiles++;
                            break;

                        case 3:
                            // place a sigil
                            // Instantiate(sigilPrefab, obstructionPos, transform.rotation, obstructionsParent);
                            // add the sigil location to the list
                            sigilLocations.Add(new Vector2Int(startX + col, startY + row));
                            break;
                        default:
                            break;
                    }
                }

            }
        }
    }

    public void ImportGridChunk(Vector2Int chunkPos, Chunk newChunk, int rotationTimes)
    {
        // get the starting positions
        int startX = chunkPos.x * 8;
        int startY = chunkPos.y * 8;

        Array2DInt newChunkGrid = RotateChunk(newChunk, rotationTimes);

        // create variable for storing the tile position in the grid.
        Vector3Int tilePos = Vector3Int.zero;

        // iterate through the specified chunk
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                // check if the current piece is an obstructed tile
                int tileInfo = newChunkGrid.GetCell(col, row);

                // if the tile isn't empty, do something
                if (tileInfo != 0)
                {
                    // find the position of the tile
                    tilePos.x = startX + col;
                    tilePos.z = startY + row;
                    Vector3 obstructionPos = grid.CellToWorld(tilePos);
                    switch (tileInfo)
                    {
                        case 1:
                            // place a wall
                            Instantiate(wallPrefab, obstructionPos, transform.rotation, obstructionsParent);
                            obstructedTiles++;
                            break;

                        case 2:
                            // place an obstruction
                            Instantiate(obstructionPrefab, obstructionPos, transform.rotation, obstructionsParent);
                            obstructedTiles++;
                            break;

                        case 3:
                            // place a sigil
                            // Instantiate(sigilPrefab, obstructionPos, transform.rotation, obstructionsParent);
                            // add the sigil location to the list
                            sigilLocations.Add(new Vector2Int(startX + col, startY + row));
                            break;
                        default:
                            break;
                    }
                }

            }
        }
    }

    private Chunk GetChunkFromFiles()
    {
        int randomChunk = UnityEngine.Random.Range(0, premadeChunks.Length);
        return premadeChunks[randomChunk];
    }

    private Array2DInt RotateChunk(Chunk chunk)
    {
        // convert chunk into an int array
        int[,] cells = chunk.chunkData.GetCells();
        int[,] rotatedChunk = new int[8, 8];

        // length of the array
        int n = cells.GetLength(0);

        // temporary chunk storage
        Array2DInt preRotatedArray = tempChunkStorage.chunkData;

        // variable to store the random number of rotations
        int randomNum;

        // choose a random number 0-3
        if (RotateTiles)
            randomNum = UnityEngine.Random.Range(0, premadeChunksCount);
        else
            // rotate tiles to be right side up
            randomNum = 1;

        // don't try to rotate, since 0 rotations are taking place
        if (randomNum == 0)
        {
            // copy the new chunk back into an Array2DBool variable
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    preRotatedArray.SetCell(i, j, cells[i, j]);
                }
            }
        }
        // rotate the chunk
        else
        {
            for (int rTimes = 0; rTimes < randomNum; rTimes++)
            {
                // rotate array by 90 degrees
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        rotatedChunk[i, j] = cells[n - j - 1, i];
                    }
                }
                Array.Copy(rotatedChunk, cells, cells.Length);
            }
            // copy the new chunk back into an Array2DBool variable
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    preRotatedArray.SetCell(i, j, rotatedChunk[i, j]);
                }
            }
        }

        // rotate the chunk
        return preRotatedArray;
    }

    public int[,] GetRandomConvertedChunk()
    {
        Array2DInt temp = RotateChunk(GetChunkFromFiles());
        return temp.GetCells();
    }

    public void FillLevel(int width, int height)
    {
        // clear tiles from the grid
        for (int i = obstructionsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(obstructionsParent.GetChild(i).gameObject);
        }
        for (int i = SigilParent.childCount - 1; i >= 0; i--)
        {
            Destroy(SigilParent.GetChild(i).gameObject);
        }

        // create digital grid variable
        DigitalGrid dGrid = new DigitalGrid();
        // set the width and height
        dGrid.SetUpGrid(width, height);

        // fill the grid with chunks
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                dGrid.PlaceChunk(GetRandomConvertedChunk(), new Vector2Int(col, row));
            }
        }

        // verify the grid
        DResult results = dGrid.Verify();
        if (results == null)
        {
            int timesRun = 0;
            while (results == null)
            {
                timesRun++;
                // fill the grid with chunks
                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        dGrid.PlaceChunk(GetRandomConvertedChunk(), new Vector2Int(col, row));
                    }
                }
                results = dGrid.Verify();
                // bandaid patch for when the start point is covered over
                if (results != null && results.resultOfBFS[dGrid.GetTileIndex(results.startPoint.x, results.startPoint.y)] != 0)
                {
                    Debug.Log("bug happened");
                    results = null;
                }
            }
            Debug.Log("Reran " + timesRun + " time(s)");
        }

        // save the int grid
        resultingGrid = results.resultOfBFS;
        dGrid.grid = ConvertIntGrid(resultingGrid);
        // set the public dGrid to the result of the verified grid
        this.dGrid = dGrid;

        // spawn tiles
        for (int row = 0; row < height * 8; row++)
        {

            for (int col = 0; col < width * 8; col++)
            {
                int temp = results.resultOfBFS[dGrid.GetTileIndex(col, row)];
                switch (temp)
                {
                    case 1:
                        // place a wall
                        Instantiate(wallPrefab, CellToWorldPos(col, row), transform.rotation, obstructionsParent);
                        obstructedTiles++;
                        break;

                    case 2:
                        // place an obstruction
                        Instantiate(obstructionPrefab, CellToWorldPos(col, row), transform.rotation, obstructionsParent);
                        obstructedTiles++;
                        break;
                    default:
                        break;
                }
            }
        }

        List<int> possible = Enumerable.Range(1, results.sigilPoints.Count).ToList();
        List<int> randomSigils = new List<int>();
        //Debug.Log(results.sigilPoints.Count);
        for (int i = 0; i < results.sigilCount; i++)
        {
            int index = UnityEngine.Random.Range(0, possible.Count);
            randomSigils.Add(possible[index]);
            possible.RemoveAt(index);
        }
        for (int i = 0; i < randomSigils.Count; i++)
        {
            //Debug.Log(i + " sigilpoints: " + randomSigils.Count);
            Instantiate(sigilPrefab, CellToWorldPos(results.sigilPoints[i]), transform.rotation, SigilParent);
        }

        Instantiate(foundTilePrefab, CellToWorldPos(results.startPoint), transform.rotation, obstructionsParent);
        Vector2Int doorPos = results.endPoints[UnityEngine.Random.Range(0, results.endPoints.Count)];
        Instantiate(doorPrefab, CellToWorldPos(doorPos), transform.rotation, obstructionsParent);
    }

    public DTile[] ConvertIntGrid(int[] grid)
    {
        DTile[] newGrid = new DTile[grid.Length];

        // convert grid into Digital Grid
        for (int row = 0; row < height * 8; row++)
        {
            for (int col = 0; col < width * 8; col++)
            {
                newGrid[col + row * width * 8] = new DTile(grid[col + row * width * 8], new Vector2Int(col, row));
            }
        }

        return newGrid;
    }

    private Array2DInt RotateChunk(Chunk chunk, int rotationTimes)
    {
        // convert chunk into an int array
        int[,] cells = chunk.chunkData.GetCells();
        int[,] rotatedChunk = new int[8, 8];

        // length of the array
        int n = cells.GetLength(0);

        // temporary chunk storage
        Array2DInt preRotatedArray = tempChunkStorage.chunkData;

        // don't try to rotate, since 0 rotations are taking place
        if (rotationTimes == 0)
        {
            // copy the new chunk back into an Array2DBool variable
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    preRotatedArray.SetCell(i, j, cells[i, j]);
                }
            }
        }
        // rotate the chunk
        else
        {
            for (int rTimes = 0; rTimes < rotationTimes; rTimes++)
            {
                // rotate array by 90 degrees
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        rotatedChunk[i, j] = cells[n - j - 1, i];
                    }
                }
                Array.Copy(rotatedChunk, cells, cells.Length);
            }
            // copy the new chunk back into an Array2DBool variable
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    preRotatedArray.SetCell(i, j, rotatedChunk[i, j]);
                }
            }
        }

        // rotate the chunk
        return preRotatedArray;
    }
    #endregion

    #region Accessing the Grid
    public Vector3 CellToWorldPos(int x, int y)
    {
        return grid.CellToWorld(new Vector3Int(x, 0, y));
    }
    public Vector3 CellToWorldPos(Vector2Int position)
    {
        return grid.CellToWorld(new Vector3Int(position.x, 0, position.y));
    }

    public Vector2Int WorldToCellPos(Vector3 pos)
    {
        Vector3Int vector3Temp = grid.WorldToCell(pos);
        Vector2Int temp = new Vector2Int(vector3Temp.x, vector3Temp.z);
        return temp;
    }

    public bool GetTileObstructed(int x, int y)
    {
        Vector3 rayPos = grid.CellToWorld(new Vector3Int(x, 0, y));
        rayPos.y = transform.position.y + 4;

        RaycastHit hit;

        if (Physics.Raycast(rayPos, Vector3.down, out hit, 3))
        {
            if (hit.collider.CompareTag("Obstruction"))
            {
                return true;
            }
        }
        return false;
    }

    public bool GetTileObstructed(Vector2Int pos)
    {
        Vector3 rayPos = grid.CellToWorld(new Vector3Int(pos.x, 0, pos.y));
        rayPos.y = transform.position.y + 4;

        RaycastHit hit;

        if (Physics.Raycast(rayPos, Vector3.down, out hit, 3))
        {
            if (hit.collider.CompareTag("Obstruction"))
            {
                return true;
            }
        }
        return false;
    }

    // tells if the position passed through is within the bounds of the grid
    public bool IsPosValid(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width * 8 || y >= height * 8)
            return false;
        return true;
    }

    public bool IsPosValid(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= width * 8 || pos.y >= height * 8)
            return false;
        return true;
    }

    public Vector2Int GetChunkLocation(Vector2Int pos)
    {
        int x = pos.x / 8;
        int y = pos.y / 8;

        return new Vector2Int(x, y);
    }

    public Vector2Int GetChunkLocation(int x, int y)
    {
        return new Vector2Int(x % 8, y % 8);
    }

    #endregion


    #region Verify
    private IEnumerator VerifyGrid(/*out StartTileInfo tileInfo*/)
    {
        // tileInfo = new StartTileInfo();

        // track all of the start tile options
        List<StartTileInfo> possibleStartTiles = new List<StartTileInfo>();

        List<Vector2Int> untestedStartTiles = new List<Vector2Int>();
        List<Vector2Int> untestedEndTiles = new List<Vector2Int>();

        // check that there are open spots on the top and bottom of the grid
        for (int i = 0; i < width * 8; i++)
        {
            // check the bottom row
            if (!GetTileObstructed(i, 0))
            {
                untestedStartTiles.Add(new Vector2Int(i, 0));
                Debug.Log("found start tile");
            }
            // check the top row
            if (!GetTileObstructed(i, height * 8 - 1))
            {
                untestedEndTiles.Add(new Vector2Int(i, height * 8 - 1));
                Debug.Log("found end tile");
            }
        }

        // if there is no space in the top or bottom of the grid, return false
        if (untestedEndTiles.Count < 1 || untestedStartTiles.Count < 1)
        {
            Debug.Log("No space in top or bottom");
            //return false;
        }


        while (untestedStartTiles.Count > 0)
        {
            for (int i = SigilParent.childCount - 1; i >= 0; i--)
            {
                Destroy(SigilParent.GetChild(i).gameObject);
            }
            // create tiles to search through
            List<Tile> tiles = new List<Tile>();
            for (int row = 0; row < height * 8; row++)
            {
                for (int col = 0; col < width * 8; col++)
                {
                    Tile temp = new(new Vector2Int(col, row), false);
                    tiles.Add(temp);
                }
            }

            StartTileInfo tempInfo = new StartTileInfo();
            tempInfo.tiles = tiles;

            // choose a random starting position
            int randTileIndex = UnityEngine.Random.Range(0, untestedStartTiles.Count);
            Vector2Int startTile = untestedStartTiles[randTileIndex];
            // take that out of the possible start tiles
            untestedStartTiles.RemoveAt(randTileIndex);
            // run the BST from that position
            // queue to store discovered tiles
            Queue<Tile> foundTiles = new Queue<Tile>();

            // store the number of found tiles
            int numFoundTiles = 0;

            foundTiles.Enqueue(tiles[startTile.x]);
            foundTiles.Peek().found = true;
            numFoundTiles++;

            // run BFS on one of the random tiles
            while (foundTiles.Count > 0)
            {


                Tile tile = foundTiles.Dequeue();
                Vector2Int pos = tile.posInGrid;

                // uncomment this and turn this function into a coroutine to show how the algorithm goes through the board
                yield return new WaitForSeconds(timeBetweenIncrement);
                Instantiate(foundTilePrefab, CellToWorldPos(pos) + Vector3.up, transform.rotation, SigilParent);

                Vector2Int upTile = pos + new Vector2Int(0, 1);
                Vector2Int downTile = pos + new Vector2Int(0, -1);
                Vector2Int leftTile = pos + new Vector2Int(-1, 0);
                Vector2Int rightTile = pos + new Vector2Int(1, 0);

                int upTileIndex = upTile.x + upTile.y * 8 * width;
                int downTileIndex = downTile.x + downTile.y * 8 * width;
                int leftTileIndex = leftTile.x + leftTile.y * 8 * width;
                int rightTileIndex = rightTile.x + rightTile.y * 8 * width;

                #region Check Surrounding Tiles
                // check if the tile that is being checked is at a valid position and that it hasn't been found
                // check the tile directly above
                if (IsPosValid(upTile) && !GetTileObstructed(upTile) && !tiles[upTileIndex].found)
                {
                    // set the tile to found
                    tiles[upTileIndex].found = true;
                    // enqueue the tile
                    foundTiles.Enqueue(tiles[upTileIndex]);
                    // increase the count of found tiles
                    numFoundTiles++;
                }
                else
                {
                    if (upTileIndex >= 0 && upTileIndex < height * 8)
                    {
                        Debug.Log("up tile failed: isPosValid: " + IsPosValid(upTile) + " isTileObstructed: "
                        + !GetTileObstructed(upTile) + " already found: " + !tiles[upTileIndex].found + " Position: " + tiles[upTileIndex].posInGrid);
                    }
                }
                // check the tile directly below
                if (IsPosValid(downTile) && !GetTileObstructed(downTile) && !tiles[downTileIndex].found)
                {
                    // set the tile to found
                    tiles[downTileIndex].found = true;
                    // enqueue the tile
                    foundTiles.Enqueue(tiles[downTileIndex]);
                    // increase the count of found tiles
                    numFoundTiles++;
                }
                else
                {
                    if (downTileIndex >= 0)
                    {
                        Debug.Log("down tile failed: isPosValid: " + IsPosValid(downTile) + " isTileObstructed: "
                        + !GetTileObstructed(downTile) + " already found: " + !tiles[downTileIndex].found + " Position: " + tiles[downTileIndex].posInGrid);
                    }

                }
                // check the tile directly to the left
                if (IsPosValid(leftTile) && !GetTileObstructed(leftTile) && !tiles[leftTileIndex].found)
                {
                    // set the tile to found
                    tiles[leftTileIndex].found = true;
                    // enqueue the tile
                    foundTiles.Enqueue(tiles[leftTileIndex]);
                    // increase the count of found tiles
                    numFoundTiles++;
                }
                else
                {
                    if (leftTileIndex >= 0)
                    {
                        Debug.Log("left tile failed: isPosValid: " + IsPosValid(leftTile) + " isTileObstructed: "
                                                + !GetTileObstructed(leftTile) + " already found: " + !tiles[leftTileIndex].found + " Position: " + tiles[leftTileIndex].posInGrid);
                    }

                }
                // check the tile directly to the right
                if (IsPosValid(rightTile) && !GetTileObstructed(rightTile) && !tiles[rightTileIndex].found)
                {
                    // set the tile to found
                    tiles[rightTileIndex].found = true;
                    // enqueue the tile
                    foundTiles.Enqueue(tiles[rightTileIndex]);
                    // increase the count of found tiles
                    numFoundTiles++;
                }
                else
                {

                    if (rightTileIndex >= 0 && rightTileIndex < width * 8)
                    {
                        Debug.Log("right tile failed: isPosValid: " + IsPosValid(rightTile) + " isTileObstructed: "
                                                + !GetTileObstructed(rightTile) + " already found: " + !tiles[rightTileIndex].found + " Position: " + tiles[rightTileIndex].posInGrid);
                    }

                }
                #endregion

            }

            /*for (int i = 0; i < tiles.Count; i++)
            {
                Debug.Log("Location: " + tiles[i].posInGrid + ", " + tiles[i].found);
            }*/

            bool hasValidTile = false;
            for (int i = 0; i < untestedEndTiles.Count; i++)
            {
                // check if a valid tile in the final row was reached
                int tileIndex = untestedEndTiles[i].x + untestedEndTiles[i].y * 8 * width;
                Debug.Log(tileIndex);
                if (tiles[tileIndex].found)
                {
                    hasValidTile = true;
                    Debug.Log("Has valid path");
                }

            }

            int chunkCount = width * height;
            if (chunkCount < 7)
            {
                tempInfo.sigilCount = 2;
            }
            else if (chunkCount < 13)
            {
                tempInfo.sigilCount = 5;
            }
            else
            {
                tempInfo.sigilCount = 6;
            }

            List<Vector2Int> validSigilLocations = new List<Vector2Int>();
            // check the sigil locations
            for (int i = 0; i < sigilLocations.Count; i++)
            {
                int tileIndex = sigilLocations[i].x + sigilLocations[i].y * 8 * width;

                // check the tile is found and its not in the same chunk as the start tile
                if (tiles[tileIndex].found /*&& GetChunkLocation(sigilLocations[i]) != GetChunkLocation(startTile)*/)
                {
                    // add sigil location to the list
                    tempInfo.sigilLocations.Add(sigilLocations[i]);
                    Debug.Log("Valid Sigil Location Found");
                }
                else
                {
                    Debug.Log("Tile at index: " + tileIndex + " has grid location of " + tiles[tileIndex].posInGrid + " or " + sigilLocations[i] + " found: " + tiles[tileIndex].found);
                }
            }

            Debug.Log(tempInfo.sigilLocations.Count);

            // check for valid door locations
            for (int i = 0; i < untestedEndTiles.Count - 1; i++)
            {
                // find the index of the tile
                int tileIndex = untestedEndTiles[i].x + untestedEndTiles[i].y * 8;
                // check if the tile location is found and if it has 2x1 tiles free
                if (tiles[tileIndex].found && untestedEndTiles[i].x + 1 == untestedEndTiles[i + 1].x)
                {
                    // add the location to the tempInfo list
                    tempInfo.doorLocations.Add(untestedEndTiles[i]);
                    Debug.Log("added door location");
                }
                else
                {
                    Debug.Log("left tile: " + untestedEndTiles[i].x + "right tile: " + untestedEndTiles[i + 1].x);
                }
            }

            // add it to the list of possible starting positions, because it passed the valid end tile test
            if (hasValidTile && tempInfo.sigilLocations.Count >= tempInfo.sigilCount)
            {
                possibleStartTiles.Add(tempInfo);
            }
            else
            {
                Debug.Log("StartTile Stats: Had Valid Path: " + hasValidTile + " SigilLocations: " + validSigilLocations.Count + " Sigil Count: " + tempInfo.sigilCount);
            }


            // check the untested starting tiles for being found, remove them if so
            for (int i = 0; i < untestedStartTiles.Count; i++)
            {
                // if the BST built from the current start point hits a point on the bottom row, remove it from the bottom row list
                if (tiles[untestedStartTiles[i].x].found)
                {
                    untestedStartTiles.RemoveAt(i);
                    // decrement to stay at the correct index
                    i--;
                }
            }
            Debug.Log("Tiles left to test: " + untestedStartTiles.Count);
        }

        // if no start tiles were added, return false
        if (possibleStartTiles.Count < 1)
        {
            Debug.Log("No Start Tiles Added");
            //return false;
        }


        // get the best starting tile
        int bestTileIndex = 0;
        for (int i = 0; i < possibleStartTiles.Count; i++)
        {
            if (possibleStartTiles[i].tileCount > possibleStartTiles[bestTileIndex].tileCount)
            {
                bestTileIndex = i;
            }
        }

        Debug.Log("Best Tile is at index:" + bestTileIndex);

        //tileInfo = possibleStartTiles[bestTileIndex];
        /*        // run BST one more time on the best starting tile and then copy the BST out
                List<Tile> bTiles = new List<Tile>();
                for (int row = 0; row < height * 8; row++)
                {
                    for (int col = 0; col < width * 8; col++)
                    {
                        Tile temp = new(new Vector2Int(col, row), false);
                        bTiles.Add(temp);
                    }
                }

                // run the BST from that position
                // queue to store discovered tiles
                Queue<Tile> queue = new Queue<Tile>();

                // store the number of found tiles
                int numFTiles = 0;

                // enqueue the best starting tile
                queue.Enqueue(bTiles[possibleStartTiles[bestTileIndex].startPos.x]);
                queue.Peek().found = true;
                numFTiles++;

                // run BFS on one of the random tiles
                while (queue.Count > 0)
                {
                    Tile tile = queue.Dequeue();
                    Vector2Int pos = tile.posInGrid;

                    // uncomment this and turn this function into a coroutine to show how the algorithm goes through the board
                    // yield return new WaitForSeconds(timeBetweenIncrement);
                    // Instantiate(foundTilePrefab, CellToWorldPos(pos) + Vector3.up, transform.rotation, SigilParent);

                    Vector2Int upTile = pos + new Vector2Int(0, 1);
                    Vector2Int downTile = pos + new Vector2Int(0, -1);
                    Vector2Int leftTile = pos + new Vector2Int(-1, 0);
                    Vector2Int rightTile = pos + new Vector2Int(1, 0);

                    int upTileIndex = upTile.x + upTile.y * 8 * width;
                    int downTileIndex = downTile.x + downTile.y * 8 * width;
                    int leftTileIndex = leftTile.x + leftTile.y * 8 * width;
                    int rightTileIndex = rightTile.x + rightTile.y * 8 * width;

                    #region Check Surrounding Tiles
                    // check if the tile that is being checked is at a valid position and that it hasn't been found
                    // check the tile directly above
                    if (IsPosValid(upTile) && !GetTileObstructed(upTile) && !bTiles[upTileIndex].found)
                    {
                        // set the tile to found
                        bTiles[upTileIndex].found = true;
                        // enqueue the tile
                        queue.Enqueue(bTiles[upTileIndex]);
                        // increase the count of found tiles
                        numFTiles++;
                    }
                    // check the tile directly below
                    if (IsPosValid(downTile) && !GetTileObstructed(downTile) && !bTiles[downTileIndex].found)
                    {
                        // set the tile to found
                        bTiles[downTileIndex].found = true;
                        // enqueue the tile
                        queue.Enqueue(bTiles[downTileIndex]);
                        // increase the count of found tiles
                        numFTiles++;
                    }
                    // check the tile directly to the left
                    if (IsPosValid(leftTile) && !GetTileObstructed(leftTile) && !bTiles[leftTileIndex].found)
                    {
                        // set the tile to found
                        bTiles[leftTileIndex].found = true;
                        // enqueue the tile
                        queue.Enqueue(bTiles[leftTileIndex]);
                        // increase the count of found tiles
                        numFTiles++;
                    }
                    // check the tile directly to the right
                    if (IsPosValid(rightTile) && !GetTileObstructed(rightTile) && !bTiles[rightTileIndex].found)
                    {
                        // set the tile to found
                        bTiles[rightTileIndex].found = true;
                        // enqueue the tile
                        queue.Enqueue(bTiles[rightTileIndex]);
                        // increase the count of found tiles
                        numFTiles++;
                    }
                    #endregion
                }

                // assign the output
                tileInfo.tiles = bTiles;
                tileInfo.startPos = possibleStartTiles[bestTileIndex].startPos;*/

        //return true;
    }

    #endregion

    private void PlaceObjects(StartTileInfo info)
    {
        // set the start position
        levelManager.SetStartLocation(info.startPos);

        List<Vector2Int> sigilLocations = new List<Vector2Int>();
        // copy sigil locations
        for (int i = 0; i < info.sigilLocations.Count; i++)
        {
            sigilLocations.Add(new Vector2Int(info.sigilLocations[i].x, info.sigilLocations[i].y));
        }
        // choose sigil locations
        for (int i = 0; i < sigilLocations.Count && i < info.sigilCount; i++)
        {
            // get a random sigil location and spawn a sigil
            int rand = UnityEngine.Random.Range(0, sigilLocations.Count);
            Instantiate(sigilPrefab, CellToWorldPos(sigilLocations[rand]), transform.rotation, obstructionsParent);
            // remove that location from the list
            sigilLocations.RemoveAt(rand);
        }

        // choose random door location
        int randomNum = UnityEngine.Random.Range(0, info.doorLocations.Count);
        Debug.Log("door length " + info.doorLocations.Count);
        Instantiate(doorPrefab, CellToWorldPos(info.doorLocations[randomNum]), transform.rotation, SigilParent);

    }

    // runs a modified version of breadth first search to discover all tiles that are accessible to the player
    /*private void VerifyGrid()
    {
        // find all the points on the bottom and top row that are not obstructed
        List<Vector2Int> bottomRow = new List<Vector2Int>();
        List<Vector2Int> topRow = new List<Vector2Int>();

        // lists to store what tile is started at and how many tiles are accessible from that tile
        List<Vector2Int> startPositions = new List<Vector2Int>();
        int tileCount = 0;

        List<Tile> savedFoundTiles = new List<Tile>();
        Vector2Int bestStartPos;

        // determine which tiles are not obstructed
        for (int i = 0; i < width * 8; i++)
        {
            // check the bottom row
            if (!GetTileObstructed(i, 0))
            {
                bottomRow.Add(new Vector2Int(i, 0));
            }
            // check the top row
            if (!GetTileObstructed(i, height * 8 - 1))
            {
                topRow.Add(new Vector2Int(i, height * 8 - 1));
            }
        }

        *//*        // if the generated map has no space on the top or bottom, regenerate the grid and verify it again
                if (bottomRow.Count == 0 || topRow.Count < 2)
                {
                    redoCount++;
                    return;
                }*//*

        int randNum = UnityEngine.Random.Range(0, bottomRow.Count);
        Vector2Int currentStartPos = bottomRow[randNum];
        bestStartPos = currentStartPos;

        // take the current start position out of the list of untested starting tiles and place it in the list tested of starting tiles
        bottomRow.Remove(currentStartPos);

        while (bottomRow.Count > 0)
        {
            // Debug.Log(bottomRow.Count);
            for (int i = SigilParent.childCount - 1; i >= 0; i--)
            {
                Destroy(SigilParent.GetChild(i).gameObject);
            }

            // create tiles to search through
            List<Tile> tiles = new List<Tile>();
            for (int row = 0; row < height * 8; row++)
            {
                for (int col = 0; col < width * 8; col++)
                {
                    Tile temp = new(new Vector2Int(col, row), false);
                    tiles.Add(temp);
                    *//*Debug.Log("Tile with position: " + col + "," + row + " created at index " + tiles.IndexOf(temp));*//*
                }
            }

            // queue to store discovered tiles
            Queue<Tile> foundTiles = new Queue<Tile>();

            // store the number of found tiles
            int numFoundTiles = 0;

            foundTiles.Enqueue(tiles[currentStartPos.x]);
            foundTiles.Peek().found = true;
            numFoundTiles++;

            // run BFS on one of the random tiles
            while (foundTiles.Count > 0)
            {
                Tile tile = foundTiles.Dequeue();
                Vector2Int pos = tile.posInGrid;

                // uncomment this and turn this function into a coroutine to show how the algorithm goes through the board
                //yield return new WaitForSeconds(timeBetweenIncrement);
                //Instantiate(foundTilePrefab, CellToWorldPos(pos) + Vector3.up, transform.rotation, SigilParent);

                Vector2Int upTile = pos + new Vector2Int(0, 1);
                Vector2Int downTile = pos + new Vector2Int(0, -1);
                Vector2Int leftTile = pos + new Vector2Int(-1, 0);
                Vector2Int rightTile = pos + new Vector2Int(1, 0);

                int upTileIndex = upTile.x + upTile.y * 8 * width;
                int downTileIndex = downTile.x + downTile.y * 8 * width;
                int leftTileIndex = leftTile.x + leftTile.y * 8 * width;
                int rightTileIndex = rightTile.x + rightTile.y * 8 * width;

                #region Check Surrounding Tiles
                // check if the tile that is being checked is at a valid position and that it hasn't been found
                // check the tile directly above
                if (IsPosValid(upTile) && !GetTileObstructed(upTile) && !tiles[upTileIndex].found)
                {
                    // set the tile to found
                    tiles[upTileIndex].found = true;
                    // enqueue the tile
                    foundTiles.Enqueue(tiles[upTileIndex]);
                    // increase the count of found tiles
                    numFoundTiles++;
                }
                // check the tile directly below
                if (IsPosValid(downTile) && !GetTileObstructed(downTile) && !tiles[downTileIndex].found)
                {
                    // set the tile to found
                    tiles[downTileIndex].found = true;
                    // enqueue the tile
                    foundTiles.Enqueue(tiles[downTileIndex]);
                    // increase the count of found tiles
                    numFoundTiles++;
                }
                // check the tile directly to the left
                if (IsPosValid(leftTile) && !GetTileObstructed(leftTile) && !tiles[leftTileIndex].found)
                {
                    // set the tile to found
                    tiles[leftTileIndex].found = true;
                    // enqueue the tile
                    foundTiles.Enqueue(tiles[leftTileIndex]);
                    // increase the count of found tiles
                    numFoundTiles++;
                }
                // check the tile directly to the right
                if (IsPosValid(rightTile) && !GetTileObstructed(rightTile) && !tiles[rightTileIndex].found)
                {
                    // set the tile to found
                    tiles[rightTileIndex].found = true;
                    // enqueue the tile
                    foundTiles.Enqueue(tiles[rightTileIndex]);
                    // increase the count of found tiles
                    numFoundTiles++;
                }
                #endregion
            }


            // check if the BFS found any of the tiles on the top row of the grid
            bool[] topTiles = new bool[topRow.Count];
            int topTilesAccessible = 0;
            for (int i = 0; i < topRow.Count; i++)
            {
                // find the index of the tile
                int tileIndex = topRow[i].x + topRow[i].y * 8;
                if (tiles[tileIndex].found)
                {
                    topTiles[i] = true;
                    topTilesAccessible++;
                }
            }

            *//*            if (topTilesAccessible < 2)
                        {
                            redoCount++;
                            return;
                        }*//*

            // check if the current BST hits another start point in the list
            for (int i = 0; i < bottomRow.Count; i++)
            {
                // if the BST built from the current start point hits a point on the bottom row, remove it from the bottom row list
                if (tiles[bottomRow[i].x].found)
                {
                    bottomRow.RemoveAt(i);
                    // decrement to stay at the correct index
                    i--;
                }
            }

            // compare the best run
            Debug.Log("numFoundTiles: " + numFoundTiles + " tileCount: " + tileCount);
            if (numFoundTiles > tileCount)
            {
                // set the count
                tileCount = numFoundTiles;
                // keep the bigger BST
                savedFoundTiles = tiles;
                bestStartPos = currentStartPos;

                Debug.Log("Updating found tiles");
            }

            if (bottomRow.Count > 0)
            {
                // choose another random start point
                randNum = UnityEngine.Random.Range(0, bottomRow.Count);
                //Debug.Log("Count: " + bottomRow.Count + " RandNum: " + randNum);
                currentStartPos = bottomRow[randNum];
            }
            // use when displaying BST
            *//*// destroy all found tile markers
            for (int i = SigilParent.childCount - 1; i >= 0; i--)
            {
                Destroy(SigilParent.GetChild(i).gameObject);
            }*//*
        }

        if (savedFoundTiles.Count == 0)
        {
            redoCount++;
            return;
        }


        // display the best start tile
        Instantiate(foundTilePrefab, CellToWorldPos(bestStartPos) + Vector3.up, transform.rotation, SigilParent);

        List<Vector2Int> validSigilLocations = new List<Vector2Int>();
        Vector2Int startChunk = GetChunkLocation(bestStartPos);
        // check each sigil location
        for (int i = 0; i < sigilLocations.Count; i++)
        {
            // get the index of the tile
            int tileIndex = sigilLocations[i].x + sigilLocations[i].y * 8 * width;
            // if the sigil location was found during the BST, add it to the list of valid sigils
            // don't include the sigil that is in the starting chunk
            Debug.Log("tileIndex: " + tileIndex + " savedFoundTiles; " + savedFoundTiles.Count);
            Debug.Log("Sigil at index " + i + " found is " + savedFoundTiles[tileIndex].found +
                " chunk location " + GetChunkLocation(sigilLocations[i]) + " Actual Location " + sigilLocations[i]
                + "Location Stored in Tile: " + savedFoundTiles[tileIndex].posInGrid);

            if (savedFoundTiles[tileIndex].found && GetChunkLocation(sigilLocations[i]) != startChunk)
            {
                validSigilLocations.Add(sigilLocations[i]);
                Debug.Log("found valid location");
            }
        }

        // choose a random sigil to be the first spawned
        int randSigil = UnityEngine.Random.Range(0, validSigilLocations.Count);
        int chunkCount = width * height;
        int sigilCount;
        if (chunkCount < 7)
        {
            sigilCount = 2;
        }
        else if (chunkCount < 13)
        {
            sigilCount = 5;
        }
        else
        {
            sigilCount = 6;
        }

        // spawn the first sigil
        Instantiate(sigilPrefab, CellToWorldPos(validSigilLocations[randSigil]), transform.rotation, obstructionsParent);

        validSigilLocations.RemoveAt(randSigil);
        for (int i = 0; i < sigilCount - 1; i++)
        {
            // do something about distance between sigils
            // spawn another random sigil
            randSigil = UnityEngine.Random.Range(0, validSigilLocations.Count);
            //Debug.Log(randSigil + "  validSigilLocations count " + validSigilLocations.Count);
            Debug.Log("Random number: " + randSigil + " number of sigil locations" + validSigilLocations.Count);
            Instantiate(sigilPrefab, CellToWorldPos(validSigilLocations[randSigil]), transform.rotation, obstructionsParent);

            validSigilLocations.RemoveAt(randSigil);
        }


        // list to store valid 2x1 areas at the top of the screen
        List<Vector2Int> doorPositions = new List<Vector2Int>();
        // find a 2x1 area at the top for the exit door
        for (int i = 0; i < topRow.Count - 1; i++)
        {

            int tileIndex = topRow[i].x + topRow[i].y * 8 * width;
            // check if there is a valid tile to the right of the current index
            // and check if that tile was found in the BST
            if (topRow[i].x == (topRow[i + 1].x - 1))
            {
                doorPositions.Add(topRow[i]);
            }
        }

        // choose a random position from the list and spawn a door there
        int randomDoor = UnityEngine.Random.Range(0, doorPositions.Count);
        Vector2Int doorPosition = doorPositions[randomDoor];
        // sDebug.Log(randomDoor + "   " + doorPositions.Count);
        Instantiate(doorPrefab, CellToWorldPos(doorPosition), transform.rotation, SigilParent);

        // set positions

        levelManager.SetSigilRequirement(sigilCount);
        levelManager.SetStartLocation(bestStartPos);
        levelManager.SetDoorLocation(doorPosition);

        // Generate Enemy Locations
        successfullyGenerated = true;
    }*/

    //

    private void OnGUI()
    {
        if (GUILayout.Button("make new layout"))
        {
            FillWholeGrid();
        }
        if (GUILayout.Button("make new layout"))
        {
            StartCoroutine(CheckAllTiles());
        }
        if (GUILayout.Button("Data Grid"))
        {
            FillLevel(width, height);
        }
        if (GUILayout.Button("Go Next Level"))
        {
            levelManager.GoToNextLevel();
        }
        if (GUILayout.Button("Test PQ"))
        {
            DTilePriorityQueue PQ = new DTilePriorityQueue();

            PQ.Pop();

            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            PQ.Insert(new DTile(UnityEngine.Random.Range(0, 100)));
            Debug.Log(PQ.PrintDist());
            PQ.Pop();
            Debug.Log(PQ.PrintDist());

        }
    }
}
