using UnityEngine;

public class Tile
{
    public Vector2Int posInGrid;
    public bool found;

    public Tile(Vector2Int pos, bool f)
    {
        this.posInGrid = pos;
        this.found = f;
    }
}
