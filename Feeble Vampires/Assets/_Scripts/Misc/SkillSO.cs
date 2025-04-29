using UnityEngine;


[CreateAssetMenu(fileName = "SkillObject", menuName = "ScriptableObjects/SkillObject")]
public class SkillSO : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int SkillLevel { get; set; }
    [field: SerializeField] public string SkillDescription { get; private set; }
    [field: SerializeField] public AudioClip Sound { get; private set; }
}
