using R3;
using UnityEngine;
using Zenject;

public class ScreenOpener : MonoBehaviour
{
    [Inject] private readonly ScreenService _screenService;
    [Inject] private readonly InputService _inputService;
    
    private readonly CompositeDisposable _disposables = new();

    private void OnEnable()
    {
        _inputService.InventoryPressed.Subscribe(_ => OnInventoryScreenChanged()).AddTo(_disposables);
    }
    
    private void OnDestroy()
    {
        _disposables?.Dispose();
    }
    
    private void OnInventoryScreenChanged()
    {
        _screenService.SwitchStateScreen(ScreenType.Inventory);
    }
}