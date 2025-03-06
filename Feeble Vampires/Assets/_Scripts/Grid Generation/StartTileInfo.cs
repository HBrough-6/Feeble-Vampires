using System.Collections.Generic;
using UnityEngine;

public class StartTileInfo
{
    public int tileCount;

    public int sigilCount;
    public Vector2Int startPos;
    public List<Vector2Int> doorLocations;
    public List<Vector2Int> sigilLocations;
    public List<Tile> tiles;

    public StartTileInfo(Vector2Int startPos, int tileCount)
    {
        this.startPos = startPos;
        this.tileCount = tileCount;
        doorLocations = new List<Vector2Int>();
        sigilLocations = new List<Vector2Int>();
    }

    public StartTileInfo(Vector2Int startPos, int tileCount, List<Vector2Int> doorLocations, List<Vector2Int> sigilLocations)
    {
        this.startPos = startPos;
        this.tileCount = tileCount;
        this.doorLocations = new List<Vector2Int>();
        this.sigilLocations = new List<Vector2Int>();
    }

    public StartTileInfo()
    {
        doorLocations = new List<Vector2Int>();
        sigilLocations = new List<Vector2Int>();
    }
}
