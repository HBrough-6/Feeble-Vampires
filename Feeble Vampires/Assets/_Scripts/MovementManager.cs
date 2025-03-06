using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public GameObject player;
    int baseCap;
    public int spaceCap;
    public int distance;
    public GameObject endPoint;

    // Heath Change
    public GridManager gridManager;
    // Heath Change
    public Vector2Int playerPosInGrid;

    public List<Vector2> pathPoints;
    Vector2 newPoint;

    public bool upBlocked;
    public bool leftBlocked;
    public bool downBlocked;
    public bool rightBlocked;

    public GameManager gameManager;

    private int maxWidth;
    private int maxHeight;

    public EnemyManager enemyManager;

    public UIManager uiManager;

    public bool hemoglobinRushing;
    public bool hanging;

    public int baseTime;
    public int timeLimit;

    private void Awake()
    {
        baseCap = 2;
        spaceCap = baseCap;
        baseTime = 10;
        timeLimit = baseTime;
        distance = 0;
        initializeOrigin();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        gameManager = FindObjectOfType<GameManager>();

        // Heath Change
        gridManager = FindObjectOfType<GridManager>();
        // Heath Change
        playerPosInGrid = new Vector2Int(0, 0);

        maxWidth = (gridManager.width * 8) - 1;
        maxHeight = (gridManager.height * 8) - 1;

        uiManager = FindObjectOfType<UIManager>();

        gameManager.resetTimer(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (distance < 1) endPoint.transform.position = new Vector3
                (endPoint.transform.position.x, -1, endPoint.transform.position.z);
        else endPoint.transform.position = new Vector3(endPoint.transform.position.x, 0.5f, endPoint.transform.position.z);

        movementBlocked();

        if (gameManager.playerHealth > 0)
        {
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
            if (endPoint.transform.position.z == maxHeight) upBlocked = true;
            else upBlocked = false;

            if (endPoint.transform.position.x == 0) leftBlocked = true;
            else leftBlocked = false;

            if (endPoint.transform.position.z == 0) downBlocked = true;
            else downBlocked = false;

            if (endPoint.transform.position.x == maxWidth) rightBlocked = true;
            else rightBlocked = false;

            //check if tile is blocked
            if (endPoint.transform.position.x != maxHeight)
            {
                if (gridManager.GetTileObstructed(Mathf.RoundToInt(Mathf.Abs(endPoint.transform.position.x + 1)),
                Mathf.RoundToInt(endPoint.transform.position.z)))
                    rightBlocked = true;
                else rightBlocked = false;
            }
            if (endPoint.transform.position.z != 0)
            {
                if (gridManager.GetTileObstructed
                (Mathf.RoundToInt(Mathf.Abs(endPoint.transform.position.x)),
                Mathf.RoundToInt(endPoint.transform.position.z - 1)))
                    downBlocked = true;
                else downBlocked = false;
            }
            if (endPoint.transform.position.x != 0)
            {
                if (gridManager.GetTileObstructed
                (Mathf.RoundToInt(Mathf.Abs(endPoint.transform.position.x - 1)),
                Mathf.RoundToInt(endPoint.transform.position.z)))
                    leftBlocked = true;
                else leftBlocked = false;
            }
            if (endPoint.transform.position.z != maxWidth)
            {
                if (gridManager.GetTileObstructed
                (Mathf.RoundToInt(Mathf.Abs(endPoint.transform.position.x)),
                Mathf.RoundToInt(endPoint.transform.position.z + 1)))
                    upBlocked = true;
                else upBlocked = false;
            }
        }
    }

    public void resetMovement()
    {
        endPoint.transform.position = new Vector3(pathPoints[0].x, endPoint.transform.position.y, pathPoints[0].y);
        initializeOrigin();
    }

    public void setPlayerPos(Vector3 startPos)
    {
        player.transform.position = startPos;
        endPoint.transform.position = new Vector3(startPos.x, endPoint.transform.position.y, startPos.z);
    }

    public void submitMovement()
    {
        if (hemoglobinRushing)
        {
            spaceCap -= 2;
            hemoglobinRushing = false;
        }

        player.transform.position = new Vector3
            (endPoint.transform.position.x, player.transform.position.y, endPoint.transform.position.z);
        initializeOrigin();
        gameManager.resetTimer(false);
        playerPosInGrid = gridManager.WorldToCellPos(endPoint.transform.position);

        if (player.GetComponent<PlayerAbilities>().canEcholocate) uiManager.makeMap();

        hanging = false;

        enemyManager.EnemiesTakeTurn();
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

    public void startHemoglobinRush()
    {
        spaceCap += 2;
        hemoglobinRushing = true;
    }

    public void switchHangingStates()
    {
        hanging = !hanging;
    }
}
