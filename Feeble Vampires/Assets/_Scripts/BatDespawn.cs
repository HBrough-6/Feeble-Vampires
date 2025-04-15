using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDespawn : MonoBehaviour
{
    Vector2 currentPos;
    Vector2 enemyPos;
    public EnemyMovement watchedEnemy;

    public bool caught;

    // Start is called before the first frame update
    void Start()
    {
        caught = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!caught)
        {
            currentPos = new Vector2(transform.position.x, transform.position.z);
            enemyPos = new Vector2(watchedEnemy.transform.position.x, watchedEnemy.transform.position.z);
        }

        if (currentPos == enemyPos && !caught)
        {
            caught = true;
            transform.position += Vector3.down * 2;
        }
    }
}
