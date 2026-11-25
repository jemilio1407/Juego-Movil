using NaughtyAttributes;
using UnityEngine;

public class ResponsiveElement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RectTransform rectTransform;

    [Header("Mobile Anchors")]
    [SerializeField] private Vector2 mobileAnchorMin = new Vector2(0, 0);
    [SerializeField] private Vector2 mobileAnchorMax = new Vector2(0, 0);

    [Header("Tablet Anchors")]
    [SerializeField] private Vector2 tabletAnchorMin = new Vector2(0, 0);
    [SerializeField] private Vector2 tabletAnchorMax = new Vector2(0, 0);

    private ResponsiveManager _responsiveManager;
    private int _lastScreenWidth;
    private int _lastScreenHeight;

    private void Start()
    {
        _responsiveManager = ResponsiveManager.Instance;
        if (_responsiveManager != null)
            _responsiveManager.OnScreenSizeChanged.AddListener(UpdateAnchors);

        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;
        UpdateAnchors();
    }

    private void OnDestroy()
    {
        if (_responsiveManager != null)
            _responsiveManager.OnScreenSizeChanged.RemoveListener(UpdateAnchors);
    }

    private void Update()
    {
        if (_responsiveManager != null) return;

        int width = Screen.width;
        int height = Screen.height;
        if (_lastScreenWidth != width || _lastScreenHeight != height)
        {
            _lastScreenWidth = width;
            _lastScreenHeight = height;
            UpdateAnchors();
        }
    }

    public void UpdateAnchors()
    {
        if (rectTransform == null) return;

        DeviceType deviceType = _responsiveManager != null
            ? _responsiveManager.CurrentDeviceType
            : GetDeviceType(Screen.width, Screen.height);

        if (deviceType == DeviceType.Mobile)
            SetMobileAnchors();
        else
            SetTabletAnchors();
    }

    private static DeviceType GetDeviceType(int width, int height)
    {
        int minDimension = Mathf.Min(width, height);
        float aspectRatio = (float)Mathf.Max(width, height) / minDimension;
        return minDimension >= 600 && aspectRatio < 2f ? DeviceType.Tablet : DeviceType.Mobile;
    }
    private void SetTabletAnchors()
    {
        rectTransform.anchorMin = tabletAnchorMin;
        rectTransform.anchorMax = tabletAnchorMax;
    }


    private void SetMobileAnchors()
    {
        rectTransform.anchorMin = mobileAnchorMin;
        rectTransform.anchorMax = mobileAnchorMax;
    }

    [Button]
    private void SaveMobileAnchors()
    {
        Vector2 maxAnchors = rectTransform.anchorMax;
        Vector2 minAnchors = rectTransform.anchorMin;

        mobileAnchorMax = maxAnchors;
        mobileAnchorMin = minAnchors;
    }

    [Button]
    private void SaveTabletAnchors()
    {
        Vector2 maxAnchors = rectTransform.anchorMax;
        Vector2 minAnchors = rectTransform.anchorMin;

        tabletAnchorMax = maxAnchors;
        tabletAnchorMin = minAnchors;
    }



}