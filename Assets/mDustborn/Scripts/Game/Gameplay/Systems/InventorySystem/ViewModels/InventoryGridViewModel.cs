using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Inventory
{
    public class InventoryGridViewModel : ViewModel
    {
        private readonly IReadOnlyInventoryGrid _inventoryGrid;

        public ReactiveProperty<string> OwnerId { get; }
        public ReactiveProperty<Vector2Int> Size { get; }
        
        private readonly List<InventorySlotViewModel> _slotViewModels = new();
        public IReadOnlyList<InventorySlotViewModel> SlotViewModels => _slotViewModels;

        public InventoryGridViewModel(IReadOnlyInventoryGrid inventoryGrid, AssetConfigLoader configLoader, AssetSpriteLoader spriteLoader)
        {
            _inventoryGrid = inventoryGrid;

            OwnerId = new ReactiveProperty<string>(inventoryGrid.OwnerId);
            Size = new ReactiveProperty<Vector2Int>(inventoryGrid.Size);

            IReadOnlyInventorySlot[,] slots = _inventoryGrid.GetSlots();
            Vector2Int size = Size.Value;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    IReadOnlyInventorySlot slot = slots[x, y];
                    InventorySlotViewModel slotViewModel = new InventorySlotViewModel(slot, configLoader, spriteLoader);
                    _slotViewModels.Add(slotViewModel);
                    Disposables.Add(slotViewModel);
                }
            }

            Disposables.Add(inventoryGrid.SizeChanged.Subscribe(newSize =>
            {
                Size.Value = newSize;
            }));
        }
    }
}