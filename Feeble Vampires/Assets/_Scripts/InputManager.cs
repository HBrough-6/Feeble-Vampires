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

    string keyToBind;

    bool readyForBind;

    // Start is called before the first frame update
    void Start()
    {
        

        readyForBind = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyForBind)
        {

        }
    }

    public void startBinding(KeyCode keyToSet)
    {
        keyToBind = keyToSet.ToString();
        readyForBind = true;
    }

    public void setKey(KeyCode keyToBind, KeyCode newKey)
    {
        keyToBind = newKey;

        
    }
}
