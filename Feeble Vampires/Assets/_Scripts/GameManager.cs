using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverHolder;
    bool dead;
    public bool skillSelecting;
    public int playerHealth;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI failedTimers;
    public int timer;
    public int failedTimerCount;
    public float internalTimer;

    private PlayerAbilities playerAbilities;

    public bool instakilled;

    public MovementManager movementManager;

    UIManager uiManager;

    PlayerItems playerItems;
    bool selfDestruct;
    public GameObject bigSkillSelectMenu;

    // Start is called before the first frame update
    void Start()
    {
        //gameOverHolder.transform.localPosition = new Vector2(gameOverHolder.transform.localPosition.x, -2000);
        playerHealth = 3;
        dead = false;
        skillSelecting = false;
        instakilled = false;

        uiManager = FindObjectOfType<UIManager>();
        movementManager = FindObjectOfType<MovementManager>();

        timer = movementManager.timeLimit;

        playerAbilities = FindObjectOfType<PlayerAbilities>();
        playerItems = FindObjectOfType<PlayerItems>();

        bigSkillSelectMenu.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            takeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.M)) uiManager.makeMap();

        if (!dead && !skillSelecting && !movementManager.spawningBatBuddy)
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
        //gameOverHolder.transform.localPosition = new Vector2(gameOverHolder.transform.localPosition.x, 0);
        dead = true;
        gameOverHolder.SetActive(true);
        if (playerHealth != 0) playerHealth = 0;

        if (playerItems.leech) selfDestruct = true;
    }

    public void Win()
    {
        skillSelecting = true;
    }

    public void restart()
    {
        if (selfDestruct)
        {
            selfDestruct = false;
            bigSkillSelectMenu.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void leechSkillSelect(string skillToActivate)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (skillToActivate != "") playerAbilities.activateSkill(skillToActivate);
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

        if (movementManager.player.GetComponent<PlayerAbilities>().hideable)
        {
            movementManager.switchHangingStates();
        }

        if (expired)
        {
            failedTimerCount++;
            movementManager.enemyManager.EnemiesTakeTurn();
        }
    }
}
