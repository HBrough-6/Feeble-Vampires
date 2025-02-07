using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverHolder;
    public int playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        gameOverHolder.transform.localPosition = new Vector2(gameOverHolder.transform.localPosition.x, -2000);
        playerHealth = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            playerHealth--;
            if (playerHealth == 0) gameOver();
        }
    }

    public void gameOver()
    {
        gameOverHolder.transform.localPosition = new Vector2(gameOverHolder.transform.localPosition.x, 0);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void exit()
    {
        Application.Quit();
    }
}
