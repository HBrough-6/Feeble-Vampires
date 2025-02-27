using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverHolder;
    bool dead;
    public int playerHealth;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI failedTimers;
    public int timer;
    public int failedTimerCount;
    public float internalTimer;

    public MovementManager movementManager;

    UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        gameOverHolder.transform.localPosition = new Vector2(gameOverHolder.transform.localPosition.x, -2000);
        playerHealth = 3;
        dead = false;

        uiManager = FindObjectOfType<UIManager>();
        movementManager = FindObjectOfType<MovementManager>();

        timer = movementManager.timeLimit;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            takeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.M)) uiManager.makeMap();

        if (!dead)
        {
            internalTimer += Time.deltaTime;
            if (internalTimer >= 1)
            {
                timer--;
                internalTimer = 0;
            }
        }

        if (timer <= 0) resetTimer(true);

        failedTimers.text = "Timer Fails: " + failedTimerCount;
        timerText.text = "Time Left: " + timer;
    }

    public void gameOver()
    {
        gameOverHolder.transform.localPosition = new Vector2(gameOverHolder.transform.localPosition.x, 0);
        dead = true;

        if (playerHealth != 0) playerHealth = 0;
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void exit()
    {
        Application.Quit();
    }

    public void takeDamage(int dealtDamage)
    {
        playerHealth -= dealtDamage;
        if (playerHealth == 0) gameOver();
    }

    public void resetTimer(bool expired)
    {
        timer = movementManager.timeLimit;
        internalTimer = 0;

        if (expired)
        {
            failedTimerCount++;
        }
    }
}
