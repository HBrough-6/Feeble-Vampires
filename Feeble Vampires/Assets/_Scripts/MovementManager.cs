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
    // Heath Change
    private SkillRandomizer skillRandomizer;

    public List<Vector2> pathPoints;
    public List<Vector2> historicPathPoints;

    Vector2 newPoint;

    public bool upBlocked;
    public bool leftBlocked;
    public bool downBlocked;
    public bool rightBlocked;

    public GameManager gameManager;

    public int maxWidth;
    public int maxHeight;

    public EnemyManager enemyManager;

    public UIManager uiManager;

    public bool hemoglobinRushing;
    public bool hanging;

    public int baseTime;
    public int timeLimit;

    public LevelManager levelManager;

    public GameObject skillSelectionHolder;

    private int timePieceExtension;
    private int shriekTimer;

    public bool timePieceActive;
    public bool isShrieking;
    public bool canSidestep;
    public bool doping;
    public bool spawningBatBuddy;

    public GameObject batBuddy;

    int placeholderSpaceCap;

    Vector2Int startingPosInGrid;

    private void Awake()
    {
        baseCap = 2;
        spaceCap = baseCap;
        baseTime = 5;
        timeLimit = baseTime;
        distance = 0;
        initializeOrigin();
    }

    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

        gameManager = FindObjectOfType<GameManager>();

        // Heath Change
        gridManager = FindObjectOfType<GridManager>();
        // Heath Change
        playerPosInGrid = new Vector2Int(0, 0);

        skillRandomizer = FindObjectOfType<SkillRandomizer>();

        maxWidth = (gridManager.width * 8) - 1;
        maxHeight = (gridManager.height * 8) - 1;

        uiManager = FindObjectOfType<UIManager>();

        gameManager.resetTimer(false);

        // Dont need this anymore - Heath
        // skillSelectionHolder.SetActive(false);

        playerPosInGrid = new Vector2Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.z));
        startingPosInGrid = playerPosInGrid;
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
                else if (!upBlocked)
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
                else if (!leftBlocked)
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
                else if (!downBlocked)
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
                else if (!rightBlocked)
                {
                    endPoint.transform.position = new Vector3
                        (endPoint.transform.position.x + 1, endPoint.transform.position.y, endPoint.transform.position.z);
                    addPathPoint();
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                resetMovement();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                submitMovement();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                // disabled for now - heath
                // endLevel();
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
        player.transform.position = startPos + Vector3.up;
        endPoint.transform.position = new Vector3(startPos.x, endPoint.transform.position.y, startPos.z);
    }

    public void endLevel()
    {
        // skillSelectionHolder.SetActive(true);
        // prevents giving the player an extra skill at the end of the safe zone
        if (!levelManager.inSafeZone)
        {
            // heath change
            gameManager.skillSelecting = true;
            skillRandomizer.Activate();
        }
        else
        {
            levelManager.GoToNextLevel();
            Debug.Log(levelManager.inSafeZone);
        }
    }

    public void FinishSelecting()
    {
        gameManager.skillSelecting = false;
        // heath change
        // skillSelectionHolder.SetActive(false);
    }

    EnemyMovement targetedEnemy;
    float detectedDistance;
    float shortestDistance;

    public void submitMovement()
    {
        //spawn the bat buddy if it's being prepared
        if (spawningBatBuddy)
        {
            shortestDistance = 9000;
            batBuddy.GetComponent<BatDespawn>().caught = false;
            batBuddy.transform.position = endPoint.transform.position;
            Vector2Int batBuddyPos = new Vector2Int(Mathf.RoundToInt(batBuddy.transform.position.x),
                Mathf.RoundToInt(batBuddy.transform.position.z));
            spaceCap = placeholderSpaceCap;
            resetMovement();
            gameManager.resetTimer(false);
            spawningBatBuddy = false;

            for (int i = 0; i < enemyManager.enemies.Count; i++)
            {
                detectedDistance = Vector3.Distance(batBuddy.transform.position, enemyManager.enemies[i].transform.position);

                if (shortestDistance > detectedDistance)
                {
                    shortestDistance = detectedDistance;
                    targetedEnemy = enemyManager.enemies[i].enemyMovement;
                }
            }

            targetedEnemy.SetTemporaryDestination(batBuddyPos);
            batBuddy.GetComponent<BatDespawn>().watchedEnemy = targetedEnemy;
            player.GetComponent<PlayerItems>().removeItem("Bat Buddy");
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(endPoint.transform.position, Vector3.down, out hit, 1.2f))
        {
            if (player.GetComponent<PlayerAbilities>().currentlyTracking)
                player.GetComponent<PlayerAbilities>().currentlyTracking = false;

            if (hit.collider.CompareTag("Sigil"))
            {
                hit.collider.GetComponent<Sigil>().Collect();
                player.GetComponent<PlayerAbilities>().sniffEnemies();
                uiManager.makeMap(player.GetComponent<PlayerAbilities>().canEcholocate);
            }
            if (hit.collider.CompareTag("Door"))
            {
                levelManager.AttemptDoorOpen();
            }
        }

        historicPathPoints.Clear();
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector2 testPoint = pathPoints[i];
            historicPathPoints.Add(testPoint);
        }

        if (hemoglobinRushing)
        {
            spaceCap -= 2;
            hemoglobinRushing = false;
            player.GetComponent<PlayerAbilities>().toggleHemoglobinEnergy();
        }

        if (timePieceActive)
        {
            timePieceExtension--;
            if (timePieceExtension == 0)
            {
                timeLimit /= 2;
                timePieceActive = false;
            }
        }

        if (isShrieking)
        {
            shriekTimer--;
            if (shriekTimer == 0)
            {
                isShrieking = false;
            }
        }

        if (doping)
        {
            spaceCap /= 2;
            doping = false;
        }

        player.transform.position = new Vector3
            (endPoint.transform.position.x, player.transform.position.y, endPoint.transform.position.z);
        initializeOrigin();
        gameManager.resetTimer(false);
        playerPosInGrid = gridManager.WorldToCellPos(endPoint.transform.position);

        uiManager.makeMap(player.GetComponent<PlayerAbilities>().canEcholocate);

        hanging = false;

        enemyManager.EnemiesTakeTurn();

        uiManager.makeMap(player.GetComponent<PlayerAbilities>().canEcholocate);

        if (player.GetComponent<PlayerAbilities>().currentlyTracking) player.GetComponent<PlayerAbilities>().currentlyTracking = false;

        if (baseCap < spaceCap && player.GetComponent<PlayerAbilities>().isSwifter)
        {
            StartCoroutine(player.GetComponent<PlayerAbilities>().swiftStepPulse());
        }
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
        player.GetComponent<PlayerAbilities>().toggleHemoglobinEnergy();
    }

    public void switchHangingStates()
    {
        hanging = !hanging;
    }

    public void hyperExtendTime()
    {
        timeLimit *= 2;
        timePieceExtension = 3;
        timePieceActive = true;
        resetMovement();
        submitMovement();
    }

    public void startShrieking()
    {
        isShrieking = true;
        shriekTimer = 3;
        resetMovement();
        submitMovement();
    }

    public void mirageSidestep(EnemyBrain specificEnemy)
    {
        int finalPathPoint = historicPathPoints.Count - 1;

        //backtrack to the previous space in your travel path
        player.transform.position = new Vector3
            (historicPathPoints[finalPathPoint].x, player.transform.position.y, historicPathPoints[finalPathPoint].y);
        playerPosInGrid = gridManager.WorldToCellPos(player.transform.position);
        endPoint.transform.position = new Vector3(player.transform.position.x, endPoint.transform.position.y, player.transform.position.z);
        historicPathPoints.RemoveAt(finalPathPoint);

        if (historicPathPoints.Count == 0) finalPathPoint = -1;

        for (int i = 0; i < specificEnemy.enemySight.seenTilesLocations.Length; i++)
        {
            if (playerPosInGrid == specificEnemy.enemySight.seenTilesLocations[i] && finalPathPoint > 0)
            {
                //if you are still in enemy sight, and you still have spaces you moved through, do it again
                mirageSidestep(specificEnemy);
            }
            else if (playerPosInGrid == specificEnemy.enemySight.seenTilesLocations[i] && finalPathPoint == -1)
            {
                Debug.Log("Can't retreat further, I admit defeat...");
                canSidestep = false;
                specificEnemy.SpottedPlayer();
                return;
            }
        }

        pathPoints[0] = playerPosInGrid;
    }

    public void dopeDouble()
    {
        resetMovement();
        submitMovement();
        spaceCap *= 2;
        doping = true;
    }

    public void cloneReset()
    {
        player.transform.position = new Vector3(startingPosInGrid.x, player.transform.position.y, startingPosInGrid.y);
        playerPosInGrid = startingPosInGrid;
        pathPoints[0] = playerPosInGrid;
        endPoint.transform.position = new Vector3(player.transform.position.x, endPoint.transform.position.y, player.transform.position.z);
        pathPoints[0] = playerPosInGrid;
        player.GetComponent<PlayerAbilities>().clone = false;
    }

    public void prepareBatBuddy()
    {
        placeholderSpaceCap = spaceCap;
        spaceCap = 9999;
    }


    public void UpdateHeightAndWidth(int width, int height)
    {
        maxWidth = (width * 8) - 1;
        maxHeight = (height * 8) - 1;
    }
}
