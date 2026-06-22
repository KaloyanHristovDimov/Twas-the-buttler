using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsScreenBinding : MonoBehaviour
{
    [Header("Sliders on this screen")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Shared model instance")]
    public AudioSettingsModel model;

    bool _updatingUI;

    void OnEnable()
    {
        // Set slider positions from the shared model when the screen becomes active
        _updatingUI = true;

        masterSlider.value = model.master;
        bgmSlider.value = model.bgm;
        sfxSlider.value = model.sfx;

        _updatingUI = false;

        // Hook listeners (safe because we add once per enable; we remove in OnDisable)
        masterSlider.onValueChanged.AddListener(HandleMaster);
        bgmSlider.onValueChanged.AddListener(HandleBgm);
        sfxSlider.onValueChanged.AddListener(HandleSfx);
    }

    void OnDisable()
    {
        masterSlider.onValueChanged.RemoveListener(HandleMaster);
        bgmSlider.onValueChanged.RemoveListener(HandleBgm);
        sfxSlider.onValueChanged.RemoveListener(HandleSfx);
    }

    void HandleMaster(float v)
    {
        if (_updatingUI) return;
        model.SetMaster(v);
    }

    void HandleBgm(float v)
    {
        if (_updatingUI) return;
        model.SetBgm(v);
    }

    void HandleSfx(float v)
    {
        if (_updatingUI) return;
        model.SetSfx(v);
    }
}