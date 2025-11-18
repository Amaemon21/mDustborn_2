using System;
using UnityEngine;
using Zenject;

public class ScreensInitializer : MonoBehaviour
{
    [Inject] private readonly ScreenService _screenService;
    
    [SerializeField] private UIScreenEntry[] _screens;

    private void OnEnable()
    {
        SubscribeWindows();
    }

    private void OnDisable()
    {
        UnsubscribeWindows();
    }

    private void SubscribeWindows()
    {
        foreach (var entry in _screens)
        {
            _screenService.SubscribeWindow(entry.type, entry.Window);
        }
    }
    
    private void UnsubscribeWindows()
    {
        foreach (var entry in _screens)
        {
            _screenService.UnsubscribeWindow(entry.type);
        }
    }
}

[Serializable]
public class UIScreenEntry
{
    public ScreenType type;
    public UIScreen Window;
}