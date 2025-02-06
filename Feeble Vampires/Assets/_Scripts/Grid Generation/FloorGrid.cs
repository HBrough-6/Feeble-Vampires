using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    public int ChunkWidth;
    public int ChunkHeight;
    private Chunk[] premadeChunks;
    private Tile[,] grid;
    private int width;
    private int height;
    [SerializeField] private int tilesPerChunk = 8;
    public GameObject whiteTilePrefab;
    public GameObject blackTilePrefab;

    public Chunk tempChunk;

    private void Awake()
    {
        SetUpGrid(ChunkWidth, ChunkHeight);
        premadeChunks = Resources.LoadAll<Chunk>("PreMade Chunks");
    }

    // set the grid size in chunks
    public void SetUpGrid(int gridChunkWidth, int gridChunkHeight)
    {
        width = gridChunkWidth * tilesPerChunk;
        height = gridChunkHeight * tilesPerChunk;
        grid = new Tile[width, height];
        bool isWhite = true;

        for (int row = 0; row < width; row++)
        {
            isWhite = !isWhite;
            for (int col = 0; col < height; col++)
            {
                GameObject temp;
                if (isWhite)
                {
                    temp = Instantiate(whiteTilePrefab, new Vector3(col, 0, -row), Quaternion.Euler(0, 0, 0), transform);
                    temp.AddComponent<Tile>();
                }
                else
                {
                    temp = Instantiate(blackTilePrefab, new Vector3(col, 0, -row), Quaternion.Euler(0, 0, 0), transform);
                    temp.AddComponent<Tile>();
                }
                isWhite = !isWhite;
                temp.GetComponent<Tile>().SetPosInGrid(row, col);
                grid[row, col] = temp.GetComponent<Tile>();

            }
        }
    }



    //    [0,0][1,0]
    //    [0,1][1,1]
    public void ImportGridChunk(Vector2Int chunkPos, Chunk newChunk)
    {
        Vector2Int chp = chunkPos;
        int startX = chp.x * 8;
        int startY = chp.y * 8;

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                // check if the current piece is an obstructed tile
                bool obstructs = newChunk.gridChunk.GetCell(col, row);
                // set the current tile to obstructs
                grid[(row + startY), (col + startX)].SetObstructing(obstructs);
            }
        }
    }

    private Chunk GetChunkFromFiles()
    {
        int randomChunk = Random.Range(0, premadeChunks.Length);
        Debug.Log(randomChunk);
        return premadeChunks[randomChunk];
    }

    public Vector3 GetTilePositionFromGrid(int row, int col)
    {
        if (row < 0 || col < 0)
        {
            return new Vector3(-1, -1, -1);
        }
        return grid[row, col].tileLocation;
    }

    public bool GetTileObstructed(int row, int col)
    {
        return grid[row, col].TileObstructs;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Import"))
        {
            for (int chunkRow = 0; chunkRow < ChunkHeight; chunkRow++)
            {
                for (int chunkCol = 0; chunkCol < ChunkWidth; chunkCol++)
                {
                    ImportGridChunk(new Vector2Int(chunkRow, chunkCol), GetChunkFromFiles());
                }
            }
            //GetChunkFromFiles();
        }
    }
}
