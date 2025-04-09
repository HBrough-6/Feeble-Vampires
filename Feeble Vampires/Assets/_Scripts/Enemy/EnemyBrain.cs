using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    private GridManager gridManager;
    private EnemyManager enemyManager;

    private EnemyMovement enemyMovement;
    private EnemySight enemySight;
    private EnemyDeathArea enemyDeathArea;

    public Vector2Int posInGrid = new Vector2Int(-1, -1);
    public Vector2Int moveDir = Vector2Int.zero;

    public Vector2Int tempDestination;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        enemyManager = FindObjectOfType<EnemyManager>();

        enemyMovement = GetComponent<EnemyMovement>();
        enemySight = GetComponent<EnemySight>();
        enemyDeathArea = GetComponent<EnemyDeathArea>();
    }

    private void Start()
    {
        // enemyManager.AddEnemy(this);
    }

    private void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.P))
        {
            Debug.Log("Setting temporary destination");
            enemyMovement.SetTemporaryDestination(tempDestination);
        }*/
    }

    /// <summary>
    /// enemy takes a turn
    /// </summary>
    public void Activate()
    {
        enemySight.DetermineSightline();
        enemyMovement.Move();
        enemySight.DetermineSightline();
        enemyDeathArea.CheckForPlayer();
    }

    /// <summary>
    /// Kills the enemy
    /// </summary>
    public void Kill()
    {
        // tell enemy manager that you died
        enemyManager.EnemyDied(this);
    }

    public void SpottedPlayer()
    {
        if (enemyManager.gameManager.movementManager.hanging == false)
        {
            if (enemyManager.gameManager.movementManager.player.GetComponent<PlayerItems>().mirage)
            {
                enemyManager.gameManager.movementManager.mirageSidestep();
            }
            else
            {
                enemyManager.PlayerSpotted();
            }
        }
    }

    public void SetMovement(Vector2Int[] path)
    {
        enemyMovement.moveNodes = path;
    }
}
