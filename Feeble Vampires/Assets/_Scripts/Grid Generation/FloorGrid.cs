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
    public void ImportGridChunk(int ChunkSection, FloorGrid grid)
    {

    }
}
