using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private TMP_Text skillDescription;
    private Image skillSprite;

    private void Awake()
    {
        skillDescription = transform.GetChild(0).GetComponent<TMP_Text>();
        skillSprite = transform.GetChild(2).GetComponent<Image>();
    }

    public void AssignSkill(SkillSO skill)
    {
        skillDescription.text = skill.SkillDescription;
        skillSprite.sprite = skill.Icon;
    }
}
