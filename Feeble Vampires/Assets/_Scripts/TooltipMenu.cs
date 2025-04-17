using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipMenu : MonoBehaviour
{
    private List<SkillSO> skills = new List<SkillSO>();

    [SerializeField] private TextMeshPro[] skillDesc = new TextMeshPro[3];
    
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

    public void AddSkillDesc(SkillSO skilldesc)
    {
        if (skills.Count >= 3)
            return;

        skills.Add(skilldesc);

        skillDesc[skills.Count - 1].text = skills[skills.Count - 1].SkillDescription;
    }
    //function that updates the description text when items/skills are gained
}
