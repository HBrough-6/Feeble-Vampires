using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        int i = 0;
        foreach (Resolution res in AllResolutions)
        {
            i++;
            newRes = res.width.ToString() + " x " + res.height.ToString();
            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                selectedResolutionList.Add(res);
            }

            if (Screen.currentResolution.Equals(res))
            {
                selectedResolution = i;
            }
        }

        resDropdown.AddOptions(resolutionStringList);
        resDropdown.SetValueWithoutNotify(selectedResolution);
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
