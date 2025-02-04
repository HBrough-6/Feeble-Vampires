using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public TextMeshProUGUI turnCounter;
    public int turnNumber;

    // Start is called before the first frame update
    void Start()
    {
        turnNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        turnCounter.text = "Turn " + turnNumber;

        if (Input.GetKeyDown(KeyCode.P)) turnNumber++;
    }
}
