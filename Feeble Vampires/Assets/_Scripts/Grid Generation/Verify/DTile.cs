using UnityEngine;

public class DTile
{
    public float dist;
    public DTile prev;
    public Vector2Int pos;
    public Vector2Int[] adjacentTiles;

    public bool found;
    // 0 = empty, 1 = wall, 2 = small object, 3 = sigil
    public int type;

    public DTile(int type, Vector2Int pos)
    {
        this.type = type;
        this.pos = pos;

        adjacentTiles = new Vector2Int[4];
        adjacentTiles[0] = pos + Vector2Int.up;
        adjacentTiles[1] = pos + Vector2Int.down;
        adjacentTiles[2] = pos + Vector2Int.left;
        adjacentTiles[3] = pos + Vector2Int.right;

        dist = float.MaxValue;
    }

    public DTile()
    {
        dist = 0;
        prev = null;
        pos = Vector2Int.zero;
        adjacentTiles = new Vector2Int[4];
        type = 0;
        found = false;
    }

    public DTile(float dist)
    {
        this.dist = dist;
    }
}
