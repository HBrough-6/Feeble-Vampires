using System.Collections.Generic;
using UnityEngine;

public class SkillRandomizer : MonoBehaviour
{
    private PlayerAbilities playerAbilities;
    private SkillHolderManager skillHolderManager;
    private GameObject skillSelectHolder;
    private LevelManager levelManager;
    private GameManager gameManager;
    private TooltipMenu tooltipMenu;

    public SkillButton skill1Button;
    public SkillButton skill2Button;

    public List<SkillSO> skills;

    public List<SkillSO> ChosenSkills = new List<SkillSO>();

    public int buttonOneSkill;
    public int buttonTwoSkill;

    private void Awake()
    {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
        skillHolderManager = FindObjectOfType<SkillHolderManager>();
        skillSelectHolder = transform.GetChild(0).gameObject;
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();
        tooltipMenu = FindObjectOfType<TooltipMenu>();
    }

    public void Activate()
    {
        skillSelectHolder.SetActive(true);
        FillButtons();
    }

    public void Deactivate()
    {
        skillSelectHolder.SetActive(false);
        levelManager.GoToNextLevel();
        gameManager.skillSelecting = false;
    }

    public void FillButtons()
    {
        if (ChosenSkills.Count >= 3)
        {
            // player has max skills, do not add any more
            SkillsFull();
            return;
        }
        List<int> skillIndexes = new List<int>();
        // fill a list with ints 
        for (int i = 0; i < skills.Count; i++)
        {
            skillIndexes.Add(i);
        }

        // remove the indexes that each of the already chosen skills are at
        for (int i = 0; i < ChosenSkills.Count; i++)
        {
            int indexToRemove = skills.IndexOf(ChosenSkills[i]);
            Debug.Log("removed at" + indexToRemove);
            skillIndexes.RemoveAt(indexToRemove);
        }

        // choose a random number
        int skillIndex = Random.Range(0, skillIndexes.Count);
        buttonOneSkill = skillIndexes[skillIndex];
        // remove that number from the skill indexes
        skillIndexes.RemoveAt(skillIndex);

        // choose a second number
        skillIndex = Random.Range(0, skillIndexes.Count);
        buttonTwoSkill = skillIndexes[skillIndex];

        // get the skill at index 1
        SkillSO skill1 = skills[buttonOneSkill];

        // assign it to button one on the UI
        skill1Button.AssignSkill(skill1);

        // get the skill at index 2
        SkillSO skill2 = skills[buttonTwoSkill];

        // assign it to button two on the UI
        skill2Button.AssignSkill(skill2);
    }

    private void SkillsFull()
    {
        buttonOneSkill = buttonTwoSkill = -1;
    }

    public void UseButtonOne()
    {
        // if the player has too many skills
        if (buttonOneSkill == -1)
        {
            Deactivate();
            return;
        }

        if (!playerAbilities.spendPoints(2, true))
        {
            return;
        }
        // add the skill to the player's current skills
        ChosenSkills.Add(skills[buttonOneSkill]);
        // add the skill to the display
        skillHolderManager.AddSkill(skills[buttonOneSkill]);

        // activate the skill
        playerAbilities.activateSkill(skills[buttonOneSkill].DisplayName);


    }

    public void UseButtonTwo()
    {
        if (!playerAbilities.spendPoints(2, true))
        {
            return;
        }
        // if the player has too many skills
        if (buttonTwoSkill == -1 && !playerAbilities.spendPoints(2, true))
        {
            Deactivate();
            return;
        }

        // add the skill to the player's current skills
        ChosenSkills.Add(skills[buttonTwoSkill]);
        // add the skill to the display
        skillHolderManager.AddSkill(skills[buttonTwoSkill]);

        // activate the skill
        playerAbilities.activateSkill(skills[buttonTwoSkill].DisplayName);

        Deactivate();
    }

    public void skipSkillAction()
    {
        Deactivate();
    }
}
