using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "ScriptableObjects/DialogueObject")]

public class DialogueSO : ScriptableObject
{
    [field: SerializeField] public string DialogueOption { get; private set; }
}
