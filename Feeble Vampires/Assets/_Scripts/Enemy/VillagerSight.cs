using UnityEngine;

public class VillagerSight : MonoBehaviour
{
    private Vector2Int[] seenTiles;
    private Vector2Int[] negativeReset;
    public FloorGrid grid;
    private VillagerMovement villagerMovement;

    public int sightLength = 4;

    private void Awake()
    {
        grid = FindObjectOfType<FloorGrid>();
        villagerMovement = GetComponent<VillagerMovement>();
        negativeReset = new Vector2Int[4];
        negativeReset[0] = new Vector2Int(-1, -1);
        negativeReset[1] = new Vector2Int(-1, -1);
        negativeReset[2] = new Vector2Int(-1, -1);
        negativeReset[3] = new Vector2Int(-1, -1);

        seenTiles = negativeReset;
    }

    public void DetermineSightline()
    {
        // reset sight
        ResetSightLine();

        // what direction is the enemy facing
        Vector2Int sightPos = new Vector2Int(villagerMovement.posInGrid.y, villagerMovement.posInGrid.x);
        Vector2Int lookDir = villagerMovement.moveDir;

        // find all tiles in a line
        for (int i = 0; i < sightLength; i++)
        {
            sightPos += lookDir;
            // the enemy sightline is looking off the edge of the board
            if (sightPos.x >= grid.width || sightPos.y >= grid.height || sightPos.x < 0 || sightPos.y < 0)
            {
                //Debug.Log("Enemy looking off map");
                break;
            }
            // enemy sightline hit an obstructed edge
            else if (grid.GetTileObstructed(sightPos.x, sightPos.y))
            {
                // vision is obstructed, don't look at any more tiles
                //Debug.Log("Tile at " + sightPos + " is obstructed");
                break;
            }
            // tile is valid
            else
            {
                // add tile to the seen tiles
                seenTiles[i] = sightPos;
            }
        }
        // check tiles in that direction
        // check if the tile is obstructed, if obstructed stop checking
        // check if the player is on one of those tiles

        // display the sight line on the board
        DisplaySightline();
    }

    private void ResetSightLine()
    {
        /*        for (int i = 0; i < seenTiles.Length; i++)
                {

                }*/
        grid.ResetTileMat(seenTiles[0].y, seenTiles[0].x);
        Debug.Log(seenTiles[0]);
        grid.ResetTileMat(seenTiles[1].y, seenTiles[1].x);
        Debug.Log(seenTiles[1]);
        grid.ResetTileMat(seenTiles[2].y, seenTiles[2].x);
        Debug.Log(seenTiles[2]);
        grid.ResetTileMat(seenTiles[3].y, seenTiles[3].x);
        Debug.Log(seenTiles[3]);


        negativeReset[0] = new Vector2Int(-1, -1);
        negativeReset[1] = new Vector2Int(-1, -1);
        negativeReset[2] = new Vector2Int(-1, -1);
        negativeReset[3] = new Vector2Int(-1, -1);

        Debug.Log("Reset" + negativeReset[0] + " " + negativeReset[1] + " " + negativeReset[2] + " " + negativeReset[3]);

        seenTiles = negativeReset;
    }

    public void DisplaySightline()
    {
        for (int i = 0; i < seenTiles.Length && seenTiles[i] != null; i++)
        {
            grid.SetTileSeen(seenTiles[i].y, seenTiles[i].x);
        }
    }
}
