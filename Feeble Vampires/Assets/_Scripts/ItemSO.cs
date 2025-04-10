using UnityEngine;

public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int SkillLevel { get; set; }
    [field: SerializeField] public string SkillDescription { get; private set; }
}
