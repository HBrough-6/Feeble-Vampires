using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettingsManager : MonoBehaviour
{
    public TMP_Dropdown resDropdown;
    public Toggle fullscreenToggle;

    Resolution[] AllResolutions;
    private bool isFullscreen;
    private int selectedResolution;
    List<Resolution> selectedResolutionList = new List<Resolution>();

    // Start is called before the first frame update
    void Start()
    {
        isFullscreen = true;
        AllResolutions = Screen.resolutions;

        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach (Resolution res in AllResolutions)
        {
            newRes = res.width.ToString() + " x " + res.height.ToString();
            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                selectedResolutionList.Add(res);
            }
            
        }

        resDropdown.AddOptions(resolutionStringList);
    }

    public void ChangeResolution()
    {
        selectedResolution = resDropdown.value;
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, isFullscreen);
    }

    public void ChangeFullscreen()
    {
        isFullscreen = fullscreenToggle.isOn;
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, isFullscreen);
    }
}
