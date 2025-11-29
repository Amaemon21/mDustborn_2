using Inventory;
using UnityEngine;
using Zenject;

public class GameplayEntryPoint : MonoBehaviour
{
    private const string OWNER_1 = "MAIN";
    
    [Inject] private readonly GameStatePlayerPrefsProvider _gameStatePrefsProvider;
    [Inject] private readonly InventoryService _inventoryService;
    [Inject] private readonly ScreenService _screenService;
        
    [SerializeField] private InventoryItemConfig _inventoryItemConfig;

    [Inject] private readonly InventoryScreenViewModel _inventoryScreenViewModel;
        
    private string _cachedOwnerId;

    private void Awake()
    {
        _gameStatePrefsProvider.Load();

        InitializedInventory();
    }

    private void InitializedInventory()
    {
        GameStateData gameState = _gameStatePrefsProvider.GameState;

        foreach (InventoryGridData inventoryData in gameState.Inventories)
        {
            _inventoryService.RegisterInventory(inventoryData);
        }

        _inventoryScreenViewModel.OpenInventory(OWNER_1);
        _cachedOwnerId = OWNER_1;
        
        _inventoryService.AddItemsToInventory(OWNER_1, _inventoryItemConfig.Id);
    }
}