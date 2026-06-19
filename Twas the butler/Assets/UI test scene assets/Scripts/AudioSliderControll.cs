using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSliderControll : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixerGroup;
    [SerializeField] private AudioMixerVolume whichGroup;

    public void OnChangeSlider(float sliderValue)
    {
        if(whichGroup == AudioMixerVolume.Master)
        {        
            audioMixerGroup.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        }
        else if(whichGroup == AudioMixerVolume.Music)
        {        
            audioMixerGroup.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        }
        else if(whichGroup == AudioMixerVolume.Sfx)
        {        
            audioMixerGroup.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        }
        
    }
    
    private enum AudioMixerVolume
    {
    Master,
    Music,
    Sfx
    }
}
