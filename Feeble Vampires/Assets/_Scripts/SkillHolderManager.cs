using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillHolderManager : MonoBehaviour
{
    private List<SkillSO> skills = new List<SkillSO>();

    [SerializeField] private Image[] skillIcons = new Image[3];

    //Christophe addition
    [SerializeField] private TMP_Text[] skillDesc = new TMP_Text[3];


    public void AddSkill(SkillSO skill)
    {
        // cannot have more than 3 skills displaying
        if (skills.Count >= 3)
            return;

        // add the skill to the list
        skills.Add(skill);

        // assign the newest skill to an Icon
        skillIcons[skills.Count - 1].sprite = skills[skills.Count - 1].Icon;

        AddSkillDesc(skill);
    }

    //christophe addition
    public void AddSkillDesc(SkillSO skilldesc)
    {
        if (skills.Count >= 3)
            return;

        skillDesc[skills.Count - 1].text = skills[skills.Count - 1].SkillDescription;

    }
}
