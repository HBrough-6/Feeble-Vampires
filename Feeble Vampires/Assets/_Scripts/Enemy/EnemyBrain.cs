using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    private GridManager gridManager;

    private EnemyMovement enemyMovement;
    private EnemySight enemySight;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();

        enemyMovement = GetComponent<EnemyMovement>();
        enemySight = GetComponent<EnemySight>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            enemyMovement.Move();
            enemySight.DetermineSightline();
        }
    }
}
