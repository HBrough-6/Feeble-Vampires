using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MoveType
    {
        Loop,
        PingPong,
        Buddy
    }

    // creating enemy pathfinding
    // function to set temporary destination
    // uses dijkstra's algorithm to find the path
    // try to find the path with the fewest turns (wish)
    // run pingpong movement there and back
    // continue from last movePoint
    // 

    private GridManager gridManager;
    private EnemyBrain enemyBrain;

    public Vector2Int[] moveNodes;
    private int targetNode = 0;

    public float turnWeight = 0.1f;

    public MoveType moveType = MoveType.PingPong;

    private bool retracingSteps = false;

    public Vector2Int[] tempPath;
    public bool buddyRetracingSteps = false;
    public Vector2Int savedMoveDir;
    public int buddyTargetNode = 0;

    private Transform body;

    private bool rotateOnNextMove = false;

    private void Awake()
    {
        // get the grid
        gridManager = FindObjectOfType<GridManager>();
        enemyBrain = GetComponent<EnemyBrain>();

        body = transform.GetChild(0);
    }

    private void Start()
    {
        // place the enemy at the start of it's movement
        SetPosInGrid(moveNodes[0].x, moveNodes[0].y);
    }

    #region Movement

    /// <summary>
    /// moves the enemy based on their movement type
    /// </summary>
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
            case MoveType.Buddy:
                BatBuddyMovement();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Looping enemy movement
    /// </summary>
    private void LoopMove()
    {
        if (rotateOnNextMove)
        {
            RotateTowardsNextNode();
            rotateOnNextMove = false;
        }
        // move to the next position
        SetPosInGrid(enemyBrain.posInGrid.x + enemyBrain.moveDir.x, enemyBrain.posInGrid.y + enemyBrain.moveDir.y);

        // check if the current tile is the target tile
        if (enemyBrain.posInGrid == moveNodes[targetNode])
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
        if (enemyBrain.posInGrid == moveNodes[targetNode])
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
            SetPosInGrid(enemyBrain.posInGrid.x + enemyBrain.moveDir.x, enemyBrain.posInGrid.y + enemyBrain.moveDir.y);
        }
    }

    private void BatBuddyMovement()
    {
        // check if the current tile is the target tile
        // Debug.Log(enemyBrain.posInGrid + " ?= " + tempPath[buddyTargetNode]);
        if (enemyBrain.posInGrid == tempPath[buddyTargetNode])
        {
            // reached the last node in the series
            // go backwards
            if (buddyTargetNode == tempPath.Length - 1)
            {
                // Debug.Log("bat buddy movement reached end, going backwards");
                buddyRetracingSteps = true;
                buddyTargetNode--;
                // face the next node
                RotateTowardsBuddyNode();
            }
            // reached the start node after walking through the whole new path
            else if (buddyTargetNode == 0 && buddyRetracingSteps)
            {
                // Debug.Log("bat buddy movement over");
                // reset buddyRetracingSteps
                buddyRetracingSteps = false;
                // restore the saved move direction
                RotateTowardsNextNode();
                // reset the move type
                moveType = MoveType.PingPong;
            }
            else
            {
                // go up through the nodes if not retracing steps or down if you are retracing steps
                buddyTargetNode += buddyRetracingSteps ? -1 : 1;
                // Debug.Log("bat buddy movement next node");
                // face the next node
                RotateTowardsBuddyNode();

            }
        }
        else
        {
            // move to the next position
            // Debug.Log("bat buddy move");
            SetPosInGrid(enemyBrain.posInGrid.x + enemyBrain.moveDir.x, enemyBrain.posInGrid.y + enemyBrain.moveDir.y);
        }
    }
    private void RotateTowardsBuddyNode()
    {
        // find the moveDir
        // get the direction of the next node
        // get the vector of movement 
        Vector2 targetDirection = new Vector2Int(tempPath[buddyTargetNode].x, tempPath[buddyTargetNode].y) - enemyBrain.posInGrid;
        //// Debug.Log("new Direction" + targetDirection);

        // normalize movement
        targetDirection.Normalize();

        // convert Vector2 to Vector2Int
        int vectX = (int)targetDirection.x;
        int vectY = (int)targetDirection.y;
        // assign new move direction
        enemyBrain.moveDir = new Vector2Int(vectX, vectY);

        // rotate to face the MoveDir

        // facing right
        if (enemyBrain.moveDir == new Vector2Int(1, 0))
        {
            body.rotation = Quaternion.Euler(0, 90, 0);
        }
        // facing left
        else if (enemyBrain.moveDir == new Vector2Int(-1, 0))
        {
            body.rotation = Quaternion.Euler(0, -90, 0);
        }
        // facing forwards
        else if (enemyBrain.moveDir == new Vector2Int(0, 1))
        {
            body.rotation = Quaternion.Euler(0, 0, 0);
        }
        // facing backwards
        else if (enemyBrain.moveDir == new Vector2Int(0, -1))
        {
            body.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void RotateTowardsNextNode()
    {
        // find the moveDir
        // get the direction of the next node
        // get the vector of movement 
        Vector2 targetDirection = new Vector2Int(moveNodes[targetNode].x, moveNodes[targetNode].y) - enemyBrain.posInGrid;
        //// Debug.Log("new Direction" + targetDirection);

        // normalize movement
        targetDirection.Normalize();

        // convert Vector2 to Vector2Int
        int vectX = (int)targetDirection.x;
        int vectY = (int)targetDirection.y;
        // assign new move direction
        enemyBrain.moveDir = new Vector2Int(vectX, vectY);

        // rotate to face the MoveDir

        // facing right
        if (enemyBrain.moveDir == new Vector2Int(1, 0))
        {
            body.rotation = Quaternion.Euler(0, 90, 0);
        }
        // facing left
        else if (enemyBrain.moveDir == new Vector2Int(-1, 0))
        {
            body.rotation = Quaternion.Euler(0, -90, 0);
        }
        // facing forwards
        else if (enemyBrain.moveDir == new Vector2Int(0, 1))
        {
            body.rotation = Quaternion.Euler(0, 0, 0);
        }
        // facing backwards
        else if (enemyBrain.moveDir == new Vector2Int(0, -1))
        {
            body.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    #endregion

    public void SetPosInGrid(int x, int y)
    {
        enemyBrain.posInGrid = new Vector2Int(x, y);
        transform.position = gridManager.CellToWorldPos(x, y);
    }

    public void SetPosInGrid(Vector2Int pos)
    {
        enemyBrain.posInGrid = pos;
        transform.position = gridManager.CellToWorldPos(pos.x, pos.y);
    }

    // returns the node pathing for a temporary path
    public List<Vector2Int> CreatePathToPoint(Vector2Int dest)
    {
        DigitalGrid dGrid = gridManager.dGrid;
        ///////////////////////////// add 0.01 or something small to the dist when a turn is made
        DTile[] grid = gridManager.GetVerifiedGridFromResults();

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i].dist = int.MaxValue;
        }

        // dikjsta's through until the path to the point is found
        DTile startTile = grid[dGrid.GetTileIndex(enemyBrain.posInGrid)];
        // set the source's dist to 0
        startTile.dist = 0;


        // grid[gridManager.dGrid.GetTileIndex(dest)].dist = 0;
        DTilePriorityQueue PQ = new DTilePriorityQueue();
        List<DTile> visited = new List<DTile>();

        PQ.Insert(startTile);

        // Debug.Log(PQ.Count);
        while (!PQ.Empty)
        {
            // Debug.Log("1");
            // if the tile has already been visited, go to next best tile
            DTile current = PQ.Pop();
            if (visited.IndexOf(current) > -1)
            {
                continue;
            }
            // node is visited
            visited.Add(current);
            // found the destination
            if (current.pos == dest)
            {
                break;
            }
            // find the tiles of each neighbor

            // above neighbor
            if (dGrid.VerifyPos(current.adjacentTiles[0]))
            {
                DTile Neighbor = grid[dGrid.GetTileIndex(current.adjacentTiles[0])];
                // check if the tile is a wall and that it hasn't been visited already
                // Debug.Log("up neighbor - type 1:" + (Neighbor.type != 1) + " type 2: " + (Neighbor.type != 2) + " visited: " + (visited.IndexOf(Neighbor) > -1) + " index: " + visited.IndexOf(Neighbor) + " position: " + Neighbor.pos);

                if (Neighbor.type != 1 && Neighbor.type != 2 && visited.IndexOf(Neighbor) < 0)
                {
                    // Debug.Log("neighbor is not visited");
                    // add 1 to the current distance and assign that to the neighbor
                    float newDist = current.dist + 1f;
                    // Debug.Log(newDist + " < " + Neighbor.dist);
                    if (newDist < Neighbor.dist)
                    {
                        // Debug.Log("up neighbor has new dist of " + newDist);
                        Neighbor.dist = newDist;
                        Neighbor.prev = current;
                        PQ.Insert(Neighbor);
                    }
                }
            }

            // down neighbor
            if (dGrid.VerifyPos(current.adjacentTiles[1]))
            {
                DTile Neighbor = grid[dGrid.GetTileIndex(current.adjacentTiles[1])];
                // check if the tile is a wall and that it hasn't been visited already
                // Debug.Log("down neighbor - type 1:" + (Neighbor.type != 1) + " type 2: " + (Neighbor.type != 2) + " visited: " + (visited.IndexOf(Neighbor) > -1) + " index: " + visited.IndexOf(Neighbor) + " position: " + Neighbor.pos);
                if (Neighbor.type != 1 && Neighbor.type != 2 && visited.IndexOf(Neighbor) < 0)
                {
                    // add 1 to the current distance and assign that to the neighbor
                    float newDist = current.dist + 1f;
                    // Debug.Log(newDist + " < " + Neighbor.dist);
                    if (newDist < Neighbor.dist)
                    {
                        // Debug.Log("Down neighbor has new dist of " + newDist);
                        Neighbor.dist = newDist;
                        Neighbor.prev = current;
                        PQ.Insert(Neighbor);
                    }
                }
            }

            // left neighbor
            if (dGrid.VerifyPos(current.adjacentTiles[2]))
            {

                DTile Neighbor = grid[dGrid.GetTileIndex(current.adjacentTiles[2])];
                // Debug.Log("left neighbor - type 1:" + (Neighbor.type != 1) + " type 2: " + (Neighbor.type != 2) + " visited: " + (visited.IndexOf(Neighbor) > -1) + " index: " + visited.IndexOf(Neighbor) + " position: " + Neighbor.pos);
                // check if the tile is a wall and that it hasn't been visited already
                if (Neighbor.type != 1 && Neighbor.type != 2 && visited.IndexOf(Neighbor) < 0)
                {
                    // add 1 to the current distance and assign that to the neighbor
                    float newDist = current.dist + 1f;
                    // Debug.Log(newDist + " < " + Neighbor.dist);
                    if (newDist < Neighbor.dist)
                    {
                        // Debug.Log("left neighbor has new dist of " + newDist);
                        Neighbor.dist = newDist;
                        Neighbor.prev = current;
                        PQ.Insert(Neighbor);
                    }
                }
            }

            // right neighbor
            if (dGrid.VerifyPos(current.adjacentTiles[3]))
            {

                DTile Neighbor = grid[dGrid.GetTileIndex(current.adjacentTiles[3])];
                // Debug.Log("right neighbor - type 1:" + (Neighbor.type != 1) + " type 2: " + (Neighbor.type != 2) + " visited: " + (visited.IndexOf(Neighbor) > -1) + " index: " + visited.IndexOf(Neighbor) + " position: " + Neighbor.pos);
                // check if the tile is a wall and that it hasn't been visited already
                if (Neighbor.type != 1 && Neighbor.type != 2 && visited.IndexOf(Neighbor) < 0)
                {
                    // add 1 to the current distance and assign that to the neighbor
                    float newDist = current.dist + 1f;
                    // Debug.Log(newDist + " < " + Neighbor.dist);
                    if (newDist < Neighbor.dist)
                    {
                        // Debug.Log("right neighbor has new dist of " + newDist);
                        Neighbor.dist = newDist;
                        Neighbor.prev = current;
                        PQ.Insert(Neighbor);
                    }
                }
            }
        }

        // create the final path
        List<Vector2Int> tempPath = new List<Vector2Int>();
        // get the final tile
        DTile currentTile = grid[dGrid.GetTileIndex(dest)];
        // find all parent tiles
        while (currentTile.prev != null)
        {
            // insert tiles at the start of the list
            tempPath.Insert(0, currentTile.pos);
            currentTile = currentTile.prev;
        }

        List<Vector2Int> corners = new List<Vector2Int>();

        int times = 0;
        string stPath = "";
        // initialize the direction variable
        Vector2Int dir = Vector2Int.zero;
        // find the corners of the path
        for (int i = 0; i < tempPath.Count - 1; i++)
        {
            times++;
            // compare the positions of the nodes (node1 - node2)
            Vector2Int newDir = tempPath[i] - tempPath[i + 1];

            stPath += tempPath[i].ToString() + ", ";
            // if the new direction equals the old direction, they are traveling in the same direction
            // if not the same, tempPath[i] is a corner
            if (dir != newDir)
            {
                // set the current direction
                dir = newDir;
                corners.Add(tempPath[i]);
            }
        }
        corners.Add(tempPath[tempPath.Count - 1]);
        // Debug.Log("Path: " + stPath + " count: " + corners.Count);
        // return the corners as a Vector2Int array
        return corners;
    }


    public void SetTemporaryDestination(Vector2Int dest)
    {
        List<Vector2Int> path = CreatePathToPoint(dest);
        tempPath = path.ToArray();
        savedMoveDir = enemyBrain.moveDir;
        moveType = MoveType.Buddy;
        RotateTowardsBuddyNode();
    }
}
