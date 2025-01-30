using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool upBlocked;
    bool leftBlocked;
    bool downBlocked;
    bool rightBlocked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        blockedMovement();
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (upBlocked == false)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (leftBlocked == false)
                transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (downBlocked == false)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (rightBlocked == false)
                transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }
    }

    public void blockedMovement()
    {
        if (transform.position.z == 8)
        {
            upBlocked = true;
        }
        else upBlocked = false;

        if (transform.position.x == -7)
        {
            leftBlocked = true;
        }
        else leftBlocked = false;

        if (transform.position.z == -7)
        {
            downBlocked = true;
        }
        else downBlocked = false;

        if (transform.position.x == 8)
        {
            rightBlocked = true;
        }
        else rightBlocked = false;
    }
}
