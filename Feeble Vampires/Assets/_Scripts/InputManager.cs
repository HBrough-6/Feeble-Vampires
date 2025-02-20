using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode north;
    public KeyCode west;
    public KeyCode south;
    public KeyCode east;
    public KeyCode confirm;
    public KeyCode cancel;
    public KeyCode useItem;

    public GameObject titleManager;

    int tempButtonPos;

    bool readyForBind;

    // Start is called before the first frame update
    void Start()
    {
        if (north == KeyCode.None) north = KeyCode.W;
        if (west == KeyCode.None) west = KeyCode.A;
        if (south == KeyCode.None) south = KeyCode.S;
        if (east == KeyCode.None) east = KeyCode.D;
        if (confirm == KeyCode.None) confirm = KeyCode.Return;
        if (cancel == KeyCode.None) cancel = KeyCode.Escape;
        if (useItem == KeyCode.None) useItem = KeyCode.Q;

        readyForBind = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyForBind)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    titleManager.GetComponent<TitleManager>().rebindKey(tempButtonPos, keyCode);
                }
            }
        }
    }

    public void startBinding(int buttonPosition)
    {
        tempButtonPos = buttonPosition;

        readyForBind = true;
    }

    public void setKey(KeyCode keyToBind, KeyCode newKey)
    {
        keyToBind = newKey;

        
    }
}
