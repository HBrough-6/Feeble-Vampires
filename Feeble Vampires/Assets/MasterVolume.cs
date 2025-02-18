using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    [SerializeField] Slider soundSlider;

    // Start is called before the first frame update
    void Start()
    {
        AdjustVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolumeFromSlider()
    {
        AdjustVolume(soundSlider.value);
    }

    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}
