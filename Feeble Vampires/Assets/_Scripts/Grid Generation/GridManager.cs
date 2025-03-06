using System;
using System.Collections;
using System.Collections.Generic;
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

    private LevelManager levelManager;

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

    public GameObject foundTilePrefab;

    public Transform obstructionsParent;
    public Transform tilesParent;
    public Transform SigilParent;

    public int obstructedTiles;

    private Chunk[] premadeChunks;
    private int premadeChunksCount;

    public List<Vector2Int> sigilLocations;

    public bool RotateTiles = true;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        sigilLocations = new List<Vector2Int>();
        premadeChunks = Resources.LoadAll<Chunk>("PreMade Chunks");
        premadeChunksCount = premadeChunks.Length;
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Start()
    {
        GenerateGrid();
    }

    #region GridCreation

    public void GenerateGrid()
    {
        int cellHeight = height * chunkSize;
        int cellWidth = width * chunkSize;

        // reset the sigil locations
        sigilLocations = new List<Vector2Int>();

        // start at the grid Manager Position
        Vector3 cellPos = transform.position;
        Vector3Int cellPosInGrid = Vector3Int.zero;

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


        VerifyGrid();
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
        int x = pos.x % 8;
        int y = pos.y % 8;

        return new Vector2Int(x, y);
    }

    public Vector2Int GetChunkLocation(int x, int y)
    {
        return new Vector2Int(x % 8, y % 8);
    }

    #endregion

    // runs a modified version of breadth first search to discover all tiles that are accessible to the player
    private IEnumerator VerifyGrid()
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


        int randNum = UnityEngine.Random.Range(0, bottomRow.Count);
        Vector2Int currentStartPos = bottomRow[randNum];
        bestStartPos = currentStartPos;

        // take the current start position out of the list of untested starting tiles and place it in the list tested of starting tiles
        bottomRow.Remove(currentStartPos);

        while (bottomRow.Count > 0)
        {
            Debug.Log(bottomRow.Count);
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
                    /*Debug.Log("Tile with position: " + col + "," + row + " created at index " + tiles.IndexOf(temp));*/
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
            }
            #endregion

            // check if the BFS found any of the tiles on the top row of the grid
            bool[] topTiles = new bool[topRow.Count];
            for (int i = 0; i < topRow.Count; i++)
            {
                // find the index of the tile
                int tileIndex = topRow[i].x + topRow[i].y * 8;
                if (tiles[tileIndex].found)
                {
                    topTiles[i] = true;
                }
            }

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
            if (numFoundTiles > tileCount)
            {
                // set the count
                tileCount = numFoundTiles;
                // keep the bigger BST
                savedFoundTiles = tiles;
                bestStartPos = currentStartPos;
            }

            if (bottomRow.Count > 0)
            {
                // choose another random start point
                randNum = UnityEngine.Random.Range(0, bottomRow.Count);
                Debug.Log("Count: " + bottomRow.Count + " RandNum: " + randNum);
                currentStartPos = bottomRow[randNum];
            }
            // destroy all found tile markers
            for (int i = SigilParent.childCount - 1; i >= 0; i--)
            {
                Destroy(SigilParent.GetChild(i).gameObject);
            }
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
            if (savedFoundTiles[tileIndex].found && GetChunkLocation(sigilLocations[i]) != startChunk)
            {
                validSigilLocations.Add(sigilLocations[i]);
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
            Debug.Log(randSigil + "  validSigilLocations count " + validSigilLocations.Count);
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
            Debug.Log(topRow[i].x + " " + topRow[i + 1].x);
            if (topRow[i].x == (topRow[i + 1].x - 1))
            {
                Debug.Log("dooro " + i);
                doorPositions.Add(topRow[i]);
            }
        }

        // choose a random position from the list and spawn a door there
        int randomDoor = UnityEngine.Random.Range(0, doorPositions.Count);
        Debug.Log(randomDoor + "   " + doorPositions.Count);
        Instantiate(doorPrefab, CellToWorldPos(doorPositions[randomDoor]), transform.rotation, SigilParent);

        // set positions

        levelManager.SetSigilRequirement(sigilCount);
        levelManager.SetStartLocation(bestStartPos);
        levelManager.SetDoorLocation(doorPositions[randomDoor]);

        // Generate Enemy Locations

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Import"))
        {
            for (int i = SigilParent.childCount - 1; i >= 0; i--)
            {
                Destroy(SigilParent.GetChild(i).gameObject);
            }
            FillWholeGrid();
        }
        if (GUILayout.Button("Verify the grid"))
        {
            StartCoroutine(VerifyGrid());
        }
    }
}
