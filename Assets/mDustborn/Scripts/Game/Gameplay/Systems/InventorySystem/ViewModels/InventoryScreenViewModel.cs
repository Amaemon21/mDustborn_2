using R3;
using UnityEngine;

namespace Inventory
{
    public class InventoryScreenViewModel : ViewModel
    {
        private readonly InventoryService _inventoryService;
        private readonly AssetSpriteLoader _assetSpriteLoader;
        private readonly AssetConfigLoader _assetConfigLoader;

        public ReactiveProperty<InventoryGridViewModel> CurrentInventory { get; } = new(null);

        public InventoryScreenViewModel(InventoryService inventoryService, AssetSpriteLoader assetSpriteLoader, AssetConfigLoader assetConfigLoader)
        {
            _inventoryService = inventoryService;
            _assetSpriteLoader = assetSpriteLoader;
            _assetConfigLoader = assetConfigLoader;
        }

        public void OpenInventory(string ownerId)
        {
            IReadOnlyInventoryGrid inventory = _inventoryService.GetInventory(ownerId);
            
            InventoryGridViewModel gridViewModel = new InventoryGridViewModel(inventory, _assetConfigLoader, _assetSpriteLoader);
            CurrentInventory.Value = gridViewModel;
        }
    }
}