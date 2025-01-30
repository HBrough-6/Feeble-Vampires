using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public GameObject player;
    public int spaceCap;
    int distance;
    public GameObject endPoint;

    public List<Vector2> pathPoints;
    Vector2 newPoint;

    bool upBlocked;
    bool leftBlocked;
    bool downBlocked;
    bool rightBlocked;

    // Start is called before the first frame update
    void Start()
    {
        spaceCap = 2;
        distance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (distance < 1) endPoint.transform.position = new Vector3(endPoint.transform.position.x, -1, endPoint.transform.position.z);
        else endPoint.transform.position = new Vector3(endPoint.transform.position.x, 1, endPoint.transform.position.z);

        movementBlocked();

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!upBlocked)
            {

            }
        }
    }

    public void movementBlocked()
    {
        if (endPoint.transform.position.z == 8) upBlocked = true;

        if (endPoint.transform.position.x == -7) leftBlocked = true;

        if (endPoint.transform.position.z == -7) downBlocked = true;

        if (endPoint.transform.position.x == 8) rightBlocked = true;

    }

    public void initializeOrigin()
    {
        pathPoints.Clear();
        newPoint = new Vector2(endPoint.transform.position.x, endPoint.transform.position.z);
        pathPoints.Add(newPoint);
    }
}
