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

    public List<TextMeshProUGUI> buttonLabels;

    // Start is called before the first frame update
    void Start()
    {
        returnToMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void returnToMainMenu()
    {
        mainMenuHolder.SetActive(true);
        settingsMenuHolder.SetActive(false);
        audioSettingsMenuHolder.SetActive(false);
        videoSettingsMenuHolder.SetActive(false);
        keybindsMenuHolder.SetActive(false);
        tutorialMenuHolder.SetActive(false);
    }

    public void settingsMenu()
    {
        mainMenuHolder.SetActive(false);
        settingsMenuHolder.SetActive(true);
    }

    public void audioSettingsMenu()
    {
        mainMenuHolder.SetActive(false);
        settingsMenuHolder.SetActive(false);
        audioSettingsMenuHolder.SetActive(true);
    }

    public void videoSettingsMenu()
    {
        mainMenuHolder.SetActive(false);
        settingsMenuHolder.SetActive(false);
        audioSettingsMenuHolder.SetActive(false);
        videoSettingsMenuHolder.SetActive(true);
    }

    public void keybindsMenu()
    {
        mainMenuHolder.SetActive(false);
        settingsMenuHolder.SetActive(false);
        audioSettingsMenuHolder.SetActive(false);
        videoSettingsMenuHolder.SetActive(false);
        keybindsMenuHolder.SetActive(true);
    }

    public void tutorialMenu()
    {
        mainMenuHolder.SetActive(false);
        tutorialMenuHolder.SetActive(true);
    }

    public void rebindKey(int keySlot, KeyCode keyCode)
    {
        //buttonLabels[keySlot].GetComponent<TMP_Text>() = keyCode.ToString();
    }
}
