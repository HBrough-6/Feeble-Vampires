using UnityEngine;

public class Traverser : MonoBehaviour
{
    public FloorGrid grid;
    private Vector2Int posInGrid = new Vector2Int(0, 0);

    private void Start()
    {
        transform.position = grid.GetTilePositionFromGrid(posInGrid.x, posInGrid.y);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            posInGrid = new Vector2Int(posInGrid.x + 1, posInGrid.y);
            transform.position = grid.GetTilePositionFromGrid(posInGrid.x, posInGrid.y);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            posInGrid = new Vector2Int(posInGrid.x, posInGrid.y - 1);
            transform.position = grid.GetTilePositionFromGrid(posInGrid.x, posInGrid.y);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            posInGrid = new Vector2Int(posInGrid.x - 1, posInGrid.y);
            transform.position = grid.GetTilePositionFromGrid(posInGrid.x, posInGrid.y);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            posInGrid = new Vector2Int(posInGrid.x, posInGrid.y + 1);
            transform.position = grid.GetTilePositionFromGrid(posInGrid.x, posInGrid.y);
        }

    }
}
