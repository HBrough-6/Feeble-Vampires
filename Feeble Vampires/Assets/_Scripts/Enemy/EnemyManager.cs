using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GridManager gridManager;
    private List<EnemyBrain> enemies;
    private List<EnemyBrain> deadEnemies;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
        enemies = new List<EnemyBrain>();
        deadEnemies = new List<EnemyBrain>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            EnemiesTakeTurn();
        }
    }

    /// <summary>
    /// Add an enemy to the list of living enemies
    /// </summary>
    /// <param name="enemy">enemy to add</param>
    public void AddEnemy(EnemyBrain enemy)
    {
        enemies.Add(enemy);
    }

    /// <summary>
    /// Removes enemy from the list of living enemies and sets it inactive
    /// </summary>
    /// <param name="deadEnemy">The enemy that died</param>
    public void EnemyDied(EnemyBrain deadEnemy)
    {
        deadEnemies.Add(deadEnemy);
        enemies.Remove(deadEnemy);
        deadEnemy.gameObject.SetActive(false);
    }

    /// <summary>
    /// Causes all enemies to take a turn
    /// </summary>
    public void EnemiesTakeTurn()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Activate();
        }
    }
}
