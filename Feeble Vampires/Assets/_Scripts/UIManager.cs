using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public TMP_Text gridMiniMap;
    public GridManager gridManager;
    public TextMeshProUGUI xpText;

    public TMP_Text levelText;

    public PlayerAbilities player;

    Color transparent;

    // Start is called before the first frame update
    void Start()
    {
        gridMiniMap.text = "";

        gridManager = FindObjectOfType<GridManager>();

        player = FindObjectOfType<PlayerAbilities>();

        transparent = new Color(0, 0, 0, 0);
    }


    public void UpdateXP(int amount)
    {
        xpText.text = "XP Points: " + amount;
    }

    public void makeMap(bool canEcholocate)
    {
        gridMiniMap.text = "";

        string addedText;

        for (int xAxis = gridManager.width * 8 - 1; xAxis >= 0; xAxis--)
        {
            for (int yAxis = gridManager.height * 8 - 1; yAxis >= 0; yAxis--)
            {
                if (!canEcholocate)
                {
                    addedText = "<color=#00000000>\u25a0</color>";
                }
                else if (gridManager.GetTileObstructed(yAxis, xAxis))
                {
                    addedText = "<color=red>\u25a0</color>";
                }
                else if ((xAxis == yAxis) || ((xAxis + yAxis) % 2 == 0))
                {
                    addedText = "<color=white>\u25a0</color>";

                }
                else
                {
                    addedText = "<color=black>\u25a0</color>";
                }
                if (FindObjectOfType<PlayerAbilities>().currentlyTracking)
                {
                    for (int i = 0; i < FindObjectOfType<EnemyManager>().enemies.Count; i++)
                    {
                        if (xAxis == FindObjectOfType<EnemyManager>().enemies[i].posInGrid.y &&
                            yAxis == FindObjectOfType<EnemyManager>().enemies[i].posInGrid.x)
                        {
                            addedText = "<color=orange>\u25a0</color>";
                        }
                    }
                }

                var sigils = GameObject.FindObjectsOfType<Sigil>();
                for (int i = 0; i < sigils.Length; i++)
                {
                    if (xAxis == Mathf.RoundToInt(sigils[i].transform.position.z) &&
                            yAxis == Mathf.RoundToInt(sigils[i].transform.position.x))
                    {
                        addedText = "<color=purple>\u25a0</color>";
                    }
                }
                if (xAxis == Mathf.RoundToInt(GameObject.FindGameObjectWithTag("Player").transform.position.z) &&
                    yAxis == Mathf.RoundToInt(GameObject.FindGameObjectWithTag("Player").transform.position.x))
                {
                    addedText = "<color=blue>\u25a0</color>";
                }
                gridMiniMap.text += addedText;
            }
            gridMiniMap.text += "\n";
        }
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "Level " + level;
    }
}
