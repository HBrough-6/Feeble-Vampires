using System;
using Array2DEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Chunk tempChunkStorage;
    public Grid grid;

    public int height;
    public int width;

    public int chunkSize = 8;

    public GameObject whiteTile;
    public GameObject blackTile;
    public GameObject Obstruction;

    public Transform obstructionsParent;
    public Transform tilesParent;

    public int obstructedTiles;

    private Chunk[] premadeChunks;
    private int premadeChunksCount;

    public bool RotateTiles = true;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        premadeChunks = Resources.LoadAll<Chunk>("PreMade Chunks");
        premadeChunksCount = premadeChunks.Length;

    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        int cellHeight = height * chunkSize;
        int cellWidth = width * chunkSize;

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

        Array2DBool newChunkGrid = RotateChunk(newChunk);

        // create variable for storing the tile position in the grid.
        Vector3Int tilePos = Vector3Int.zero;

        // iterate through the specified chunk
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                // check if the current piece is an obstructed tile
                bool obstructs = newChunkGrid.GetCell(col, row);

                // find the position of the tile in the world
                if (obstructs)
                {
                    tilePos.x = startX + col;
                    tilePos.z = startY + row;
                    Vector3 obstructionPos = grid.CellToWorld(tilePos);

                    obstructedTiles++;


                    // create the obstruction in the world
                    Instantiate(Obstruction, obstructionPos, transform.rotation, obstructionsParent);
                }
            }
        }
    }

    private Chunk GetChunkFromFiles()
    {
        int randomChunk = UnityEngine.Random.Range(0, premadeChunks.Length);
        return premadeChunks[randomChunk];
    }

    private Array2DBool RotateChunk(Chunk chunk)
    {
        // convert chunk into a bool array
        bool[,] cells = chunk.gridChunk.GetCells();
        bool[,] rotatedChunk = new bool[8, 8];

        // length of the array
        int n = cells.GetLength(0);

        // temporary chunk storage
        Array2DBool temp = tempChunkStorage.gridChunk;

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
                    temp.SetCell(i, j, cells[i, j]);
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
                    temp.SetCell(i, j, rotatedChunk[i, j]);
                }
            }
        }

        // rotate the chunk
        return temp;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Import"))
        {
            FillWholeGrid();
        }
        if (GUILayout.Button("check Obstructed"))
        {
            Debug.Log(GetTileObstructed(0, 0));
        }
    }
    public Vector3 CellToWorldPos(int x, int y)
    {
        return grid.CellToWorld(new Vector3Int(x, 0, y));
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
}
