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
        enemyManager.AddEnemy(this);
    }

    /// <summary>
    /// enemy takes a turn
    /// </summary>
    public void Activate()
    {
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
}
