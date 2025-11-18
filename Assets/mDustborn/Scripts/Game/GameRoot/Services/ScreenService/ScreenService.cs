using System.Collections.Generic;
using UnityEngine;

public class ScreenService
{
    private readonly Dictionary<ScreenType, UIScreen> _windows = new();
    
    public void SubscribeWindow(ScreenType screenType, UIScreen window)
    {
        if (!_windows.ContainsKey(screenType))
        {
            _windows.Add(screenType, window);
            window.Close();
        }
    }
    
    public void UnsubscribeWindow(ScreenType screenType)
    {
        if (_windows.TryGetValue(screenType, out UIScreen window))
        {
            if (window.IsOpen)
            {
                window.Close(); 
            }
            
            _windows.Remove(screenType);
        }
        else
        {
            Debug.Log($"Window with type {screenType} not found");
        }
    }
    
    public void OpenWindow(ScreenType screenType)
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                window.Close();
            }
        }
        
        if (_windows.TryGetValue(screenType, out UIScreen windowToOpen))
        {
            windowToOpen.Open();
        }
        else
        {
            Debug.Log($"Window with type {screenType} not found");
        }
    }
    
    public void CloseWindow(ScreenType screenType)
    {
        if (_windows.TryGetValue(screenType, out UIScreen window))
        {
            if (window.IsOpen)
            {
                window.Close();
            }        
            else
            {
                Debug.Log($"Window with type {screenType} is already closed.");
            }
        }
        else
        {
            Debug.Log($"Window with type {screenType} not found");
        }
    }

    public void SwitchStateScreen(ScreenType screenType)
    {
        if (_windows.TryGetValue(screenType, out UIScreen windowToOpen))
        {
            if (windowToOpen.IsOpen)
            {
                windowToOpen.Close();
            }
            else
            {
                CloseAllWindows();
                
                windowToOpen.Open();
            }
        }   
    }
    
    public void CloseAllWindows()
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                window.Close();
            }
        }
    }
    
    public void CloseAndRemoveAllWindows()
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                window.Close();
            }
            
            Object.Destroy(window.gameObject);
        }
        
        _windows.Clear();
    }

    public bool IsWindowOpened(ScreenType screenType)
    {
        if (_windows.TryGetValue(screenType, out UIScreen window))
        {
            return window.IsOpen;
        }

        return false;
    }
    
    public bool HasAnyWindowOpen()
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                return true;
            }
        }
        
        return false;
    }
    
    public UIScreen GetUIScreen(ScreenType screenType)
    {
        return _windows.GetValueOrDefault(screenType);
    }
}