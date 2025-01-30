using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isObstructing;
    private Vector2Int posInGrid;
    public Vector3 tileLocation;

    public bool TileObstructs
    {
        get { return isObstructing; }
    }


    private void Start()
    {
        tileLocation = transform.position;
    }

    public void SetPosInGrid(Vector2Int pos)
    {
        posInGrid = pos;
    }

    public void SetObstructing(bool obstructs)
    {
        isObstructing = obstructs;
    }

}
