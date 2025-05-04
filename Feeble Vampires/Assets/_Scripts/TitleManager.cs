using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject settingsMenuHolder;
    public GameObject audioSettingsMenuHolder;
    public GameObject videoSettingsMenuHolder;
    public GameObject keybindsMenuHolder;
    public GameObject tutorialMenuHolder;
    public GameObject collectionsMenuHolder;
    public GameObject currentMenuHolder;
    public GameObject previousMenuHolder;

    public List<TextMeshProUGUI> buttonLabels;

    // Start is called before the first frame update
    void Start()
    {
        previousMenuHolder = mainMenuHolder;
        currentMenuHolder = mainMenuHolder;
        returnToMainMenu();
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            returntoPrevious();
        }
    }
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void returnToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void returnToMainMenu()
    {
        disableUI();
        setCurrentActive(mainMenuHolder);
    }

    public void returntoPrevious()
    {
        //Debug.Log("go back");
        currentMenuHolder.SetActive(false);
        currentMenuHolder = previousMenuHolder;
        currentMenuHolder.SetActive(true);
        previousMenuHolder = mainMenuHolder;
    }

    public void settingsMenu()
    {
        setCurrentActive(settingsMenuHolder);
    }

    public void audioSettingsMenu()
    {
        setCurrentActive(audioSettingsMenuHolder);
    }

    public void videoSettingsMenu()
    {
        setCurrentActive(videoSettingsMenuHolder);
    }

    public void keybindsMenu()
    {
        setCurrentActive(keybindsMenuHolder);
    }

    public void tutorialMenu()
    {
        setCurrentActive(tutorialMenuHolder);
    }

    public void collectionsMenu()
    {
        setCurrentActive(collectionsMenuHolder);
    }

    public void setCurrentActive(GameObject holder)
    {
        previousMenuHolder = currentMenuHolder;
        previousMenuHolder.SetActive(false);

        currentMenuHolder = holder;
        currentMenuHolder.SetActive(true);
    }

    public void disableUI()
    {
        settingsMenuHolder.SetActive(false);
        audioSettingsMenuHolder.SetActive(false);
        videoSettingsMenuHolder.SetActive(false);
        keybindsMenuHolder.SetActive(false);
        tutorialMenuHolder.SetActive(false);
        collectionsMenuHolder.SetActive(false);
    }
    public void rebindKey(int keySlot, KeyCode keyCode)
    {
        //buttonLabels[keySlot].GetComponent<TMP_Text>() = keyCode.ToString();
    }
}
