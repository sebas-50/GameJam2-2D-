using UnityEngine;
using UnityEngine.UI;

public class VolumeMusicSlider : MonoBehaviour
{
    private Slider slider;
    
    
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeVolumeWithSlider(AudioManager.Instance.musicSource, val));
    }
}