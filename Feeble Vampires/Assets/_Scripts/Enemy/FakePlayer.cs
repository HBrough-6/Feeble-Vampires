using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    private GridManager gridManager;
    private EnemyManager enemyManager;

    public Vector2Int posInGrid = new Vector2Int(8, 10);

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Start()
    {
        transform.position = gridManager.CellToWorldPos(posInGrid.x, posInGrid.y);
    }

    public void seen()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            posInGrid += new Vector2Int(0, 1);
            transform.position = gridManager.CellToWorldPos(posInGrid.x, posInGrid.y);
            enemyManager.EnemiesTakeTurn();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            posInGrid += new Vector2Int(0, -1);
            transform.position = gridManager.CellToWorldPos(posInGrid.x, posInGrid.y);
            enemyManager.EnemiesTakeTurn();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            posInGrid += new Vector2Int(1, 0);
            transform.position = gridManager.CellToWorldPos(posInGrid.x, posInGrid.y);
            enemyManager.EnemiesTakeTurn();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            posInGrid += new Vector2Int(-1, 0);
            transform.position = gridManager.CellToWorldPos(posInGrid.x, posInGrid.y);
            enemyManager.EnemiesTakeTurn();
        }
    }
}
