using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool canEcholocate;
    public UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        if (canEcholocate) uiManager.makeMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
