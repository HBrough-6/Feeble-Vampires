using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int SigilsRequiredForUnlock = 1;
    private int currentSigilsCollected = 0;

    private GridManager gridManager;

    private Vector2Int startLocation;
    private Vector2Int[] doorLocations;

    private int currentLevel = 1;

    public void GenerateLevelOne()
    {
        // reset grid
        gridManager.GenerateGrid();
    }

    public void GenerateLevelTwo()
    {
        // reset the grid
        gridManager.GenerateGrid();
    }

    public void FailLevel()
    {
        // open the fail screen
    }

    private void CompleteLevel()
    {
        // increase level count
        currentLevel++;
        if (currentLevel == 2)
        {
            // pop up Skill select Screen
        }
        else if (currentLevel >= 3)
        {
            // you win screen
        }
    }

    public void AttemptDoorOpen()
    {
        if (currentSigilsCollected >= SigilsRequiredForUnlock)
        {
            CompleteLevel();
        }
    }

    public void PickUpSigil()
    {
        currentSigilsCollected++;
    }

    public void SetSigilRequirement(int sigils)
    {
        SigilsRequiredForUnlock = sigils;
        currentSigilsCollected = 0;
    }

    public void SetStartLocation(Vector2Int sPos)
    {
        startLocation = sPos;
    }

    public void SetDoorLocation(Vector2Int dPos)
    {
        doorLocations[0] = dPos;
        doorLocations[1] = dPos + new Vector2Int(1, 0);
    }
}
