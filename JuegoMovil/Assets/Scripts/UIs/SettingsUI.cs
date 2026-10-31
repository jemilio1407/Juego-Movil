using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Dino.Utility.Audio;
using AudioType = Dino.Utility.Audio.AudioType;

public class SettingsUI : UIWindow
{
    [Header("SettingsUI - Switches")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    [Header("Switch Visuals (Sprites)")]
    [SerializeField] private Image musicSwitchImage;
    [SerializeField] private Image sfxSwitchImage;
    [SerializeField] private Sprite switchOnSprite;
    [SerializeField] private Sprite switchOffSprite;

    private void Awake()
    {
        // 1. Cargar preferencias
        bool isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        bool isSfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;

        // 2. Sincronizar los toggles y visuales
        if (musicToggle != null) musicToggle.isOn = isMusicOn;
        if (sfxToggle != null) sfxToggle.isOn = isSfxOn;
        UpdateVisuals(musicSwitchImage, isMusicOn);
        UpdateVisuals(sfxSwitchImage, isSfxOn);

        // 3. Aplicar volumen inicial
        ApplyAudioVolume(AudioType.Music, isMusicOn);
        ApplyAudioVolume(AudioType.SFX, isSfxOn);

        // 4. Conectar eventos
        if (musicToggle != null) musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        if (sfxToggle != null) sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateVisuals(musicSwitchImage, isOn);
        ApplyAudioVolume(AudioType.Music, isOn);
    }

    private void OnSfxToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("SfxOn", isOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateVisuals(sfxSwitchImage, isOn);
        ApplyAudioVolume(AudioType.SFX, isOn);

        // Reproducir sonido de prueba si se enciende
        if (AudioManager.Instance != null && isOn)
        {
            AudioManager.Instance.PlaySound("click1");
        }
    }

    private void UpdateVisuals(Image switchImage, bool isOn)
    {
        if (switchImage != null)
        {
            switchImage.sprite = isOn ? switchOnSprite : switchOffSprite;
        }
    }

    private void ApplyAudioVolume(AudioType type, bool isOn)
    {
        if (AudioManager.Instance == null) return;
        float volumeValue = isOn ? 1f : 0.0001f;
        AudioManager.Instance.UpdateAudioMixerGroupVolume(volumeValue, type);
    }

    public void OnCloseClicked()
    {
        Time.timeScale = 1f;
        Hide();
        StartCoroutine(FreezeTimeAfterHide());
    }

    private IEnumerator FreezeTimeAfterHide()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }
}