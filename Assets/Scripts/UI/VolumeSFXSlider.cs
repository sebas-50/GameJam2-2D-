using UnityEngine;
using UnityEngine.UI;

public class VolumeSFXSlider : MonoBehaviour
{
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeVolumeWithSlider(AudioManager.Instance.ambientSource, val));
        slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeVolumeWithSlider(AudioManager.Instance.catSource, val));
        slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeVolumeWithSlider(AudioManager.Instance.enemySource, val));
        slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeVolumeWithSlider(AudioManager.Instance.playerSource, val));
        slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeVolumeWithSlider(AudioManager.Instance.sfxSource, val));
        slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeVolumeWithSlider(AudioManager.Instance.uiSource, val));
        
    }
}