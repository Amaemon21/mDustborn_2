using Inventory;
using UnityEngine;
using Zenject;

public class GameplayEntryPoint : MonoBehaviour
{
    private const string OWNER_1 = "MAIN";
    
    [Inject] private readonly GameStatePlayerPrefsProvider _gameStatePrefsProvider;
    [Inject] private readonly ScreenService _screenService;
    
    [SerializeField] private InventoryView _inventoryView;
        
    [SerializeField] private InventoryItemConfig _inventoryItemConfig;
    
    private InventoryService _inventoryService;
    private InventoryScreenController _inventoryScreenController;
        
    private string _cachedOwnerId;

    private void Awake()
    {
        _gameStatePrefsProvider.Load();

        InitializedInventory();
    }

    private void InitializedInventory()
    {
        _inventoryService = new InventoryService(_gameStatePrefsProvider);
        
        GameStateData gameState = _gameStatePrefsProvider.GameState;

        foreach (InventoryGridData inventoryData in gameState.Inventories)
        {
            _inventoryService.RegisterInventory(inventoryData);
        }
        
        _inventoryScreenController = new InventoryScreenController(_inventoryService, _inventoryView);
        _inventoryScreenController.OpenInventory(OWNER_1);
        _cachedOwnerId = OWNER_1;


        _inventoryService.AddItemsToInventory(OWNER_1, _inventoryItemConfig.Id, 1);
    }
}