using UnityEngine;



public class VillagerMovement : MonoBehaviour
{

    //   posInGrid(y,x)
    //   y x->
    //   |
    //   v   [][][][][][][][]
    //       [][][][][][][][]
    //       [][][][][][][][]
    //       [][][][][][][][]
    //       [][][][][][][][]
    //       [][][][][][][][]
    //       [][][][][][][][]
    //       [][][][][][][][]

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

        // check if the tile you are at is the target tile
        if (new Vector2Int(posInGrid.y, posInGrid.x) == moveNodes[targetNode])
        {
            if (targetNode == moveNodes.Length - 1)
            {
                targetNode = 0;
            }
            else
            {
                targetNode++;
            }

            // find the next move direction and then 
            RotateTowardsNextNode();
        }
        SetPosInGrid(posInGrid.y + moveDir.x, posInGrid.x + moveDir.y);
        Debug.Log("current pos" + posInGrid + "   Target Node: " + moveNodes[targetNode]);

    }

    // used to set the enemy's position in the grid and move them there
    public void SetPosInGrid(int row, int col)
    {
        posInGrid = new Vector2Int(col, row);
        transform.position = grid.GetTilePositionFromGrid(posInGrid.x, posInGrid.y);
    }

    private void DisplayNodes()
    {
        for (int i = 0; i < moveNodes.Length; i++)
        {
            grid.SetTileObstructed(moveNodes[i].y, moveNodes[i].x);
        }
    }

    public void RotateTowardsNextNode()
    {
        // get the direction of the next node
        // get the vector of movement 
        Vector2 targetDirection = posInGrid - new Vector2Int(moveNodes[targetNode].y, moveNodes[targetNode].x);
        Debug.Log("new Direction" + targetDirection);
        // normalize movement
        targetDirection.Normalize();
        // convert Vector2 to Vector2Int
        int vectX = (int)targetDirection.x;
        int vectY = (int)targetDirection.y;
        // assign new move direction
        moveDir = new Vector2Int(-vectY, -vectX);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Move"))
        {
            InvokeRepeating("Move", 0, 0.3f);
            //Move();
        }
        if (GUILayout.Button("Display Path"))
        {
            DisplayNodes();
        }
    }
}
