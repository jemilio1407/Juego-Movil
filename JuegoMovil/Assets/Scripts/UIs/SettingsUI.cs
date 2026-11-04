using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections;
using Dino.Utility.Audio;
using AudioType = Dino.Utility.Audio.AudioType;

public class SettingsUI : UIWindow
{
    [Header("Settings Toggles")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    [Header("Switch Visuals (Sprites)")]
    [SerializeField] private Image musicSwitchImage;
    [SerializeField] private Image sfxSwitchImage;
    [SerializeField] private Sprite switchOnSprite;
    [SerializeField] private Sprite switchOffSprite;

    private void Awake()
    {
        bool isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        bool isSFXOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;

        if (musicToggle != null)
        {
            musicToggle.isOn = isMusicOn;
            UpdateSwitchVisual(musicSwitchImage, isMusicOn);
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        if (sfxToggle != null)
        {
            sfxToggle.isOn = isSFXOn;
            UpdateSwitchVisual(sfxSwitchImage, isSFXOn);
            sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
        }

        StartCoroutine(ApplyInitialAudioSettings(isMusicOn, isSFXOn));
    }

    private IEnumerator ApplyInitialAudioSettings(bool musicOn, bool sfxOn)
    {
        yield return null;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateAudioMixerGroupVolume(sfxOn ? 1f : 0.0001f, AudioType.SFX);

            if (!musicOn)
            {
                AudioManager.Instance.StopSound("BGM");
            }
        }
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSwitchVisual(musicSwitchImage, isOn);

        if (AudioManager.Instance != null)
        {
            if (isOn)
            {
                AudioManager.Instance.PlaySound("BGM");
            }
            else
            {
                AudioManager.Instance.StopSound("BGM");
            }
        }
    }

    private void OnSFXToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("SFXOn", isOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSwitchVisual(sfxSwitchImage, isOn);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateAudioMixerGroupVolume(isOn ? 1f : 0.0001f, AudioType.SFX);

            if (isOn)
            {
                AudioManager.Instance.PlaySound("click1");
            }
        }
    }

    private void UpdateSwitchVisual(Image switchImage, bool isOn)
    {
        if (switchImage != null && switchOnSprite != null && switchOffSprite != null)
        {
            switchImage.sprite = isOn ? switchOnSprite : switchOffSprite;
        }
    }

    public void OnCloseClicked()
    {
        Time.timeScale = 1f;
        Hide();
        StopAllCoroutines();

        if (GameManager.Instance != null)
        {
            StartCoroutine(RepauseAfterHideRoutine());
        }
    }

    private IEnumerator RepauseAfterHideRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0f;
    }

    #region Test Methods
    [Button("Test Show")]
    private void TestShow() { Show(); }

    [Button("Test Hide")]
    private void TestHide() { Hide(); }
    #endregion
}