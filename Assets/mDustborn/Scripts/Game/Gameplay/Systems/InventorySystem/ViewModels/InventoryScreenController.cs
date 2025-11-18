using Inventory;

public class InventoryScreenController
{
    private readonly InventoryService _inventoryService;
    private readonly InventoryView _view;

    private InventoryGridController _currentInventoryGridController;
        
    public InventoryScreenController(InventoryService inventoryService, InventoryView view)
    {
        _inventoryService = inventoryService;
        _view = view;
    }

    public void OpenInventory(string ownerId)
    {
        IReadOnlyInventoryGrid inventory = _inventoryService.GetInventory(ownerId);
        InventoryView inventoryView = _view;
            
        _currentInventoryGridController = new InventoryGridController(inventory, inventoryView);
    }
}