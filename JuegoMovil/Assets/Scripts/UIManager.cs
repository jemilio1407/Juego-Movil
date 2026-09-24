using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<UIWindow> _uiWindows;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void ShowWindow(string windowName)
    {
        foreach (var window in _uiWindows)
        {
            if(window.Id ==  windowName)
            {
                window.Show();
                break;
            }
        }
    }

    public void HideWindow(string windowName)
    {
        foreach (var window in _uiWindows)
        {
            if (window.Id == windowName)
            {
                window.Show();
                break;
            }
        }
    }
}
