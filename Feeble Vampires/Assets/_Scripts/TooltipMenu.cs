using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipMenu : MonoBehaviour
{

    public GameObject TooltipBox;

    public TextMeshPro skill1Desc;
    public TextMeshPro skill2Desc;
    public TextMeshPro skill3Desc;

    public TextMeshPro item1Desc;
    public TextMeshPro item2Desc;

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


    //needs references to the CurrentSkills list

    //function that updates the description text when items/skills are gained
}
