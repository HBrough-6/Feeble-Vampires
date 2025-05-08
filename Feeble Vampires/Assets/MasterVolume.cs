using UnityEngine;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    [SerializeField] Slider soundSlider;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("SavedMasterVolume", 0.5f);
        AdjustVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 1));
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
