using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public TMP_Text gridMiniMap;
    public GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        gridMiniMap.text = "";

        gridManager = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeMap()
    {
        gridMiniMap.text = "";

        
        for (int xAxis = gridManager.width * 8 - 1; xAxis >= 0; xAxis--)
        {
            for (int yAxis = gridManager.height * 8 - 1; yAxis >= 0; yAxis--)
            {
                if (gridManager.GetTileObstructed(yAxis, xAxis))
                {
                    gridMiniMap.text += "<color=red>\u25a0</color>";
                }
                else if (xAxis == Mathf.RoundToInt(GameObject.FindGameObjectWithTag("Player").transform.position.z) &&
                    yAxis == Mathf.RoundToInt(GameObject.FindGameObjectWithTag("Player").transform.position.x))
                {
                    gridMiniMap.text += "<color=blue>\u25a0</color>";
                }
                else if (FindObjectOfType<PlayerAbilities>().currentlyTracking)
                {
                    for (int i = 0; i < FindObjectOfType<EnemyManager>().enemies.Count; i++)
                    {
                        if (xAxis == FindObjectOfType<EnemyManager>().enemies[i].posInGrid.x &&
                            yAxis == FindObjectOfType<EnemyManager>().enemies[i].posInGrid.y)
                        {
                            gridMiniMap.text += "<color=yellow>\u25a0</color>";
                        }
                    }
                }
                else if ((xAxis == yAxis) || ((xAxis + yAxis) % 2 == 0))
                {
                    gridMiniMap.text += "<color=black>\u25a0</color>";
                }
                else
                {
                    gridMiniMap.text += "<color=white>\u25a0</color>";
                }
            }
            gridMiniMap.text += "\n";
        }
    }
}
