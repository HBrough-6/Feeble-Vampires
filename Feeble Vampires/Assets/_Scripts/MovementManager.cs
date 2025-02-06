using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public GameObject player;
    public int spaceCap;
    public int distance;
    public GameObject endPoint;

    public GameObject gridManager;

    public List<Vector2> pathPoints;
    Vector2 newPoint;

    public bool upBlocked;
    public bool leftBlocked;
    public bool downBlocked;
    public bool rightBlocked;

    // Start is called before the first frame update
    void Start()
    {
        spaceCap = 2;
        distance = 0;
        initializeOrigin();
    }

    // Update is called once per frame
    void Update()
    {
        if (distance < 1) endPoint.transform.position = new Vector3
                (endPoint.transform.position.x, -1, endPoint.transform.position.z);
        else endPoint.transform.position = new Vector3(endPoint.transform.position.x, 1, endPoint.transform.position.z);

        movementBlocked();

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (distance > 0
                && pathPoints[distance - 1] == new Vector2(endPoint.transform.position.x, endPoint.transform.position.z + 1))
            {
                endPoint.transform.position = new Vector3
                    (pathPoints[distance - 1].x, endPoint.transform.position.y, pathPoints[distance - 1].y);
                removePathPoint();
            }
            if (!upBlocked)
            {
                endPoint.transform.position = new Vector3
                    (endPoint.transform.position.x, endPoint.transform.position.y, endPoint.transform.position.z + 1);
                addPathPoint();
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (distance > 0
                && pathPoints[distance - 1] == new Vector2(endPoint.transform.position.x - 1, endPoint.transform.position.z))
            {
                endPoint.transform.position = new Vector3
                    (pathPoints[distance - 1].x, endPoint.transform.position.y, pathPoints[distance - 1].y);
                removePathPoint();
            }
            if (!leftBlocked)
            {
                endPoint.transform.position = new Vector3
                    (endPoint.transform.position.x - 1, endPoint.transform.position.y, endPoint.transform.position.z);
                addPathPoint();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (distance > 0
                && pathPoints[distance - 1] == new Vector2(endPoint.transform.position.x, endPoint.transform.position.z - 1))
            {
                endPoint.transform.position = new Vector3
                    (pathPoints[distance - 1].x, endPoint.transform.position.y, pathPoints[distance - 1].y);
                removePathPoint();
            }
            if (!downBlocked)
            {
                endPoint.transform.position = new Vector3
                    (endPoint.transform.position.x, endPoint.transform.position.y, endPoint.transform.position.z - 1);
                addPathPoint();
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (distance > 0
                && pathPoints[distance - 1] == new Vector2(endPoint.transform.position.x + 1, endPoint.transform.position.z))
            {
                endPoint.transform.position = new Vector3
                    (pathPoints[distance - 1].x, endPoint.transform.position.y, pathPoints[distance - 1].y);
                removePathPoint();
            }
            if (!rightBlocked)
            {
                endPoint.transform.position = new Vector3
                    (endPoint.transform.position.x + 1, endPoint.transform.position.y, endPoint.transform.position.z);
                addPathPoint();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            resetMovement();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            submitMovement();
        }
    }

    public void movementBlocked()
    {
        if (distance == spaceCap)
        {
            upBlocked = true;
            leftBlocked = true;
            downBlocked = true;
            rightBlocked = true;
        }
        else
        {
            if (endPoint.transform.position.z == 0) upBlocked = true;
            else upBlocked = false;

            if (endPoint.transform.position.x == 0) leftBlocked = true;
            else leftBlocked = false;

            if (endPoint.transform.position.z == (gridManager.GetComponent<FloorGrid>().ChunkWidth * -8) + 1) downBlocked = true;
            else downBlocked = false;

            if (endPoint.transform.position.x == (gridManager.GetComponent<FloorGrid>().ChunkHeight * 8) - 1) rightBlocked = true;
            else rightBlocked = false;

            //check if tile is blocked
            if (gridManager.GetComponent<FloorGrid>().GetTileObstructed
                (Mathf.RoundToInt(endPoint.transform.position.x), Mathf.RoundToInt(endPoint.transform.position.z + 1)))
                upBlocked = true;
            else upBlocked = false;

            if (gridManager.GetComponent<FloorGrid>().GetTileObstructed
                (Mathf.RoundToInt(endPoint.transform.position.x - 1), Mathf.RoundToInt(endPoint.transform.position.z)))
                leftBlocked = true;
            else leftBlocked = false;

            if (gridManager.GetComponent<FloorGrid>().GetTileObstructed
                (Mathf.RoundToInt(endPoint.transform.position.x), Mathf.RoundToInt(endPoint.transform.position.z - 1)))
                downBlocked = true;
            else downBlocked = false;

            if (gridManager.GetComponent<FloorGrid>().GetTileObstructed
                (Mathf.RoundToInt(endPoint.transform.position.x + 1), Mathf.RoundToInt(endPoint.transform.position.z)))
                rightBlocked = true;
            else rightBlocked = false;
        }
    }

    public void resetMovement()
    {
        endPoint.transform.position = new Vector3(pathPoints[0].x, endPoint.transform.position.y, pathPoints[0].y);
        initializeOrigin();
    }

    public void submitMovement()
    {
        player.transform.position = new Vector3
            (endPoint.transform.position.x, player.transform.position.y, endPoint.transform.position.z);
        initializeOrigin();
    }

    public void initializeOrigin()
    {
        pathPoints.Clear();
        newPoint = new Vector2(endPoint.transform.position.x, endPoint.transform.position.z);
        pathPoints.Add(newPoint);
        distance = 0;
    }

    public void addPathPoint()
    {
        newPoint = new Vector2(endPoint.transform.position.x, endPoint.transform.position.z);
        pathPoints.Add(newPoint);
        distance++;
    }
    public void removePathPoint()
    {
        pathPoints.RemoveAt(distance);
        distance--;
    }
}
