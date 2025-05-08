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
    LevelManager levelManager;
    bool selfDestruct;
    public GameObject bigSkillSelectMenu;

    GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        //gameOverHolder.transform.localPosition = new Vector2(gameOverHolder.transform.localPosition.x, -2000);
        playerHealth = 3;
        dead = false;
        //skillSelecting = false;
        instakilled = false;

        uiManager = FindObjectOfType<UIManager>();
        movementManager = FindObjectOfType<MovementManager>();

        timer = movementManager.timeLimit;

        playerAbilities = FindObjectOfType<PlayerAbilities>();
        playerItems = FindObjectOfType<PlayerItems>();
        levelManager = FindObjectOfType<LevelManager>();
        gridManager = FindObjectOfType<GridManager>();

        // bigSkillSelectMenu.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            takeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.M)) uiManager.gridMiniMap.gameObject.SetActive(!uiManager.gridMiniMap.isActiveAndEnabled);

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

        if (failedTimerCount > 1 && !dead) gameOver();
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
        playerAbilities.canEcholocate = false;
        playerAbilities.isSwifter = false;
        playerAbilities.canRushAttack = false;
        playerAbilities.smarter = false;
        playerAbilities.hideable = false;
        playerAbilities.isGreedy = false;
        playerAbilities.strongestInstinct = false;
        playerAbilities.scentTracker = false;
        playerAbilities.clone = false;

        playerItems.removeItem("Broken Timepiece");
        playerItems.removeItem("Shriek");
        playerItems.removeItem("Mirage");
        playerItems.removeItem("Blood Dope");
        playerItems.removeItem("Leech");
        playerItems.removeItem("Bat Buddy");

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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        levelManager.currentLevel = 0;

        gridManager.width = 2;
        gridManager.height = 2;
        gridManager.GenerateGrid(true);

        levelManager.GoToNextLevel();
        bigSkillSelectMenu.SetActive(false);
        dead = false;
        gameOverHolder.SetActive(false);
        playerHealth = 3;
        if (skillToActivate != "") playerAbilities.activateSkill(skillToActivate);

        playerItems.leechParticles.SetActive(false);
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
            movementManager.enemyManager.EnemiesTakeTurn();

            if (!movementManager.player.GetComponent<PlayerAbilities>().hideable)
            {
                failedTimerCount++;
            }
        }
    }
}
