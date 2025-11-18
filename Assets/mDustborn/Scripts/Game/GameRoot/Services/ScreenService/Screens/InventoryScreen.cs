using Zenject;

public class InventoryScreen : UIScreen
{
    [Inject] private readonly InputService _inputService;
    
    protected override void OnOpen()
    {
        _inputService.DisablePlayerMap();
        ShowCursor();
    }

    protected override void OnClose()
    {
        _inputService.EnablePlayerMap();
        HideCursor();
    }
}
