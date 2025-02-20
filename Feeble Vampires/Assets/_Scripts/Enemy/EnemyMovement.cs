using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MoveType
    {
        Loop,
        PingPong
    }

    private GridManager gridManager;


    public Vector2Int[] moveNodes;
    public Vector2Int moveDir = Vector2Int.zero;
    private int targetNode = 0;
    public Vector2Int posInGrid = new Vector2Int(-1, -1);

    public MoveType moveType = MoveType.Loop;
    private bool retracingSteps = false;

    private Transform body;

    private bool rotateOnNextMove = false;

    private void Awake()
    {
        // get the grid
        gridManager = FindObjectOfType<GridManager>();
        body = transform.GetChild(0);
    }

    private void Start()
    {
        // place the enemy at the start of it's movement
        SetPosInGrid(moveNodes[0].x, moveNodes[0].y);
    }

    #region Movement

    public void Move()
    {
        switch (moveType)
        {
            case MoveType.Loop:
                LoopMove();
                break;
            case MoveType.PingPong:
                PingPongMove();
                break;
            default:
                break;
        }
    }

    private void LoopMove()
    {
        if (rotateOnNextMove)
        {
            RotateTowardsNextNode();
            rotateOnNextMove = false;
        }
        // move to the next position
        SetPosInGrid(posInGrid.x + moveDir.x, posInGrid.y + moveDir.y);

        // check if the current tile is the target tile
        if (posInGrid == moveNodes[targetNode])
        {

            // reached the last node in the series
            if (targetNode == moveNodes.Length - 1)
            {
                targetNode = 0;
            }
            else
            {
                targetNode++;
            }

            // face the next node
            rotateOnNextMove = true;
        }
    }


    private void PingPongMove()
    {
        // check if the current tile is the target tile
        if (posInGrid == moveNodes[targetNode])
        {
            // reached the last node in the series
            // go backwards
            if (targetNode == moveNodes.Length - 1)
            {
                retracingSteps = true;
                targetNode--;
            }
            else if (targetNode == 0 && retracingSteps)
            {
                retracingSteps = false;
                targetNode++;
            }
            else
            {
                // go up through the nodes if not retracing steps or down if you are retracing steps
                targetNode += retracingSteps ? -1 : 1;
            }
            // face the next node
            RotateTowardsNextNode();
        }
        else
        {
            // move to the next position
            SetPosInGrid(posInGrid.x + moveDir.x, posInGrid.y + moveDir.y);
        }
    }

    private void RotateTowardsNextNode()
    {
        // find the moveDir
        // get the direction of the next node
        // get the vector of movement 
        Vector2 targetDirection = new Vector2Int(moveNodes[targetNode].x, moveNodes[targetNode].y) - posInGrid;
        //Debug.Log("new Direction" + targetDirection);

        // normalize movement
        targetDirection.Normalize();

        // convert Vector2 to Vector2Int
        int vectX = (int)targetDirection.x;
        int vectY = (int)targetDirection.y;
        // assign new move direction
        moveDir = new Vector2Int(vectX, vectY);

        // rotate to face the MoveDir

        // facing right
        if (moveDir == new Vector2Int(1, 0))
        {
            body.rotation = Quaternion.Euler(0, 90, 0);
        }
        // facing left
        else if (moveDir == new Vector2Int(-1, 0))
        {
            body.rotation = Quaternion.Euler(0, -90, 0);
        }
        // facing forwards
        else if (moveDir == new Vector2Int(0, 1))
        {
            body.rotation = Quaternion.Euler(0, 0, 0);
        }
        // facing backwards
        else if (moveDir == new Vector2Int(0, -1))
        {
            body.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    #endregion

    public void SetPosInGrid(int x, int y)
    {
        posInGrid = new Vector2Int(x, y);
        transform.position = gridManager.CellToWorldPos(x, y);

    }
}
