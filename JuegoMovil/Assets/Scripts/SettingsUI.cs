using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class SettingsUI : UIWindow
{
    [Header("SettingsUI")]
    [SerializeField] private Slider _volumeSlider;


    public override void Initialize()
    {
        base.Initialize();
        //_volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

    }

    private void OnVolumeChanged(float value)
    {
        Debug.Log($"Volume changed to: {value}");        
    }



    [Button("Test Show")]
    private void TestShow()
    {
        Show();
    }

    [Button("Test Hide")]
    private void TestHide()
    {
        Hide();
    }


}