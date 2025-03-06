using UnityEngine;

public class Sigil : MonoBehaviour
{
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Collect()
    {
        levelManager.PickUpSigil();
        Destroy(gameObject);
    }
}
