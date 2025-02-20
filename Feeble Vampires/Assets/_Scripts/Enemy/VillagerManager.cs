using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    private VillagerSight villagerSight;
    private VillagerMovement villagerMovement;

    private void Awake()
    {
        villagerMovement = GetComponent<VillagerMovement>();
        villagerSight = GetComponent<VillagerSight>();
    }

    private void Activate()
    {
        // call the villager
        villagerMovement.Move();
        villagerSight.DetermineSightline();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Activate"))
        {
            Activate();
        }
    }
}
