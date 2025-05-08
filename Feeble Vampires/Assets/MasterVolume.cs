using UnityEngine;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    [SerializeField] Slider soundSlider;

    public float volume;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("SavedMasterVolume", 1);
        AdjustVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 1));
    }

    // Update is called once per frame
    void Update()
    {
        volume = AudioListener.volume;
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
