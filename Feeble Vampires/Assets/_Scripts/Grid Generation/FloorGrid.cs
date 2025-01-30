using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    private Tile[][] grid;
    private int width;
    private int height;
    [SerializeField] private int tilesPerChunk = 8;

    // set the grid size in chunks
    public void SetGridSize(int gridChunkWidth, int gridChunkHeight)
    {
        grid = new Tile[gridChunkWidth * tilesPerChunk][];
        for (int i = 0; i < gridChunkWidth; i++)
        {
            grid[i] = new Tile[gridChunkHeight * tilesPerChunk];
        }
    }

    //    [1][2]
    //    [3][4]
    public void ImportGridChunk(int ChunkSection, Chunk grid)
    {

    }

    public Vector3 GetTilePositionFromGrid(int x, int y)
    {
        return grid[x][y].tileLocation;
    }

    public bool GetTileObstructed(int x, int y)
    {
        return grid[x][y].TileObstructs;
    }
}
