using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    public int ChunkWidth;
    public int ChunkHeight;
    private Tile[,] grid;
    private int width;
    private int height;
    [SerializeField] private int tilesPerChunk = 8;
    public GameObject whiteTilePrefab;
    public GameObject blackTilePrefab;


    private void Awake()
    {
        SetUpGrid(ChunkWidth, ChunkHeight);
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
                    temp = Instantiate(whiteTilePrefab, new Vector3(col, 0, row), Quaternion.Euler(0, 0, 0), transform);
                    temp.AddComponent<Tile>();
                }
                else
                {
                    temp = Instantiate(blackTilePrefab, new Vector3(col, 0, row), Quaternion.Euler(0, 0, 0), transform);
                    temp.AddComponent<Tile>();
                }
                isWhite = !isWhite;
                temp.GetComponent<Tile>().SetPosInGrid(row, col);
                grid[row, col] = temp.GetComponent<Tile>();

            }
        }
    }



    //    [1][2]
    //    [3][4]
    public void ImportGridChunk(int ChunkSection, Chunk grid)
    {

    }

    public Vector3 GetTilePositionFromGrid(int row, int col)
    {
        return grid[row, col].tileLocation;
    }

    public bool GetTileObstructed(int row, int col)
    {
        return grid[row, col].TileObstructs;
    }
}
