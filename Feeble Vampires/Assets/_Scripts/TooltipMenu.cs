using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipMenu : MonoBehaviour
{
    
    public GameObject TooltipBox;

    void Awake()
    {
        TooltipBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            TooltipBox.SetActive(true);
        }
        else
        {
            TooltipBox.SetActive(false);
        }
    }

}
