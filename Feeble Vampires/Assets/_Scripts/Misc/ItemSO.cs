using UnityEngine;


[CreateAssetMenu(fileName = "ItemObject", menuName = "ScriptableObjects/ItemObject")]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string ItemDescription { get; private set; }
    [field: SerializeField] public AudioClip Sound { get; private set; }
}
