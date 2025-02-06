using UnityEngine;



public class VillagerMovement : MonoBehaviour
{

    public FloorGrid grid;

    public Vector2Int[] moveNodes;

    public Vector2Int moveDir = Vector2Int.zero;
    private int targetNode;
    public Vector2Int posInGrid = new Vector2Int(-1, -1);

    private void Awake()
    {
        // get the floor grid of the scene
        grid = FindObjectOfType<FloorGrid>();

    }

    private void Start()
    {
        SetPosInGrid(moveNodes[0].x, moveNodes[0].y);
    }

    public void Move()
    {
        // check if the node you are at is the target Node
        if (posInGrid == moveNodes[targetNode])
        {
            Debug.Log("jgdfg");
            if (targetNode == moveNodes.Length - 1)
            {
                targetNode = 0;
            }
            else
            {
                targetNode++;
            }
            // find the next move direction and then 
            moveDir = RotateTowardsNextNode();
        }
        // check for player
        else
        {
            // move in the currentDirection
            SetPosInGrid(posInGrid.y + moveDir.x, posInGrid.x + moveDir.y);
        }



    }

    // used to set the enemy's position in the grid and move them there
    public void SetPosInGrid(int row, int col)
    {
        posInGrid = new Vector2Int(col, row);
        transform.position = grid.GetTilePositionFromGrid(posInGrid.x, posInGrid.y);
    }

    public Vector2Int RotateTowardsNextNode()
    {
        // get the move direction
        Vector2Int td = moveNodes[targetNode] - moveDir;
        Debug.Log(td);
        if (td.y < 0)
        {
            td = new Vector2Int(1, 0);
        }
        else
        {
            td = new Vector2Int(-1, 0);
        }

        if (td.x < 0)
        {
            td = new Vector2Int(0, 1);
        }
        else
        {
            td = new Vector2Int(0, -1);
        }

        return td;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("move"))
        {
            Move();
        }
    }
}
