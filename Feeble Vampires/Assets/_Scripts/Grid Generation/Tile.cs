using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isObstructing;
    private Vector2Int posInGrid;
    public Vector3 tileLocation;
    public Material red;
    public Material defaultMat;

    private Renderer ChildRenderer;

    public bool TileObstructs
    {
        get { return isObstructing; }
    }


    private void Start()
    {
        tileLocation = transform.position;
        ChildRenderer = GetComponentInChildren<Renderer>();
        defaultMat = ChildRenderer.material;
    }

    public void SetPosInGrid(int row, int col)
    {
        posInGrid = new Vector2Int(col, row);
    }

    public void SetObstructing(bool obstructs)
    {
        isObstructing = obstructs;
        if (isObstructing)
        {
            SetMat(red);
        }
        else
        {
            ResetMat();
        }
    }

    public void SetMat(Material mat)
    {
        ChildRenderer.material = mat;
    }

    public void ResetMat()
    {
        ChildRenderer.material = defaultMat;
    }
}
