using UnityEngine;

public class EnemyDeathArea : MonoBehaviour
{
    private Vector2Int[] deathTiles;
    private MovementManager player;
    private EnemyBrain enemyBrain;

    private void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
        player = FindObjectOfType<MovementManager>();

        deathTiles = new Vector2Int[8];

        deathTiles[0] = new Vector2Int(0, 1);
        deathTiles[1] = new Vector2Int(1, 1);
        deathTiles[2] = new Vector2Int(1, 0);
        deathTiles[3] = new Vector2Int(1, -1);
        deathTiles[4] = new Vector2Int(0, -1);
        deathTiles[5] = new Vector2Int(-1, -1);
        deathTiles[6] = new Vector2Int(-1, 0);
        deathTiles[7] = new Vector2Int(-1, -1);
    }

    public void CheckForPlayer()
    {
        int sightedTile = findSightedTile();
        for (int i = 0; i < deathTiles.Length; i++)
        {
            if (player.playerPosInGrid == deathTiles[i] + enemyBrain.posInGrid)
            {
                if (i == sightedTile)
                {
                    enemyBrain.SpottedPlayer();
                }
                else
                {
                    enemyBrain.Kill();
                }
                break;
            }
        }
    }

    private int findSightedTile()
    {
        Vector2Int moveDir = enemyBrain.moveDir;

        for (int i = 0; i < deathTiles.Length; i++)
        {
            if (deathTiles[i] + enemyBrain.posInGrid == enemyBrain.posInGrid + moveDir)
            {
                return i;
            }
        }
        return 0;
    }
}
