using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryGridController
    {
        private readonly List<InventoryViewModel> _slotControllers = new();
        
        public InventoryGridController(IReadOnlyInventoryGrid inventory, InventoryView view)
        {
             Vector2Int size = inventory.Size;
             IReadOnlyInventorySlot[,] slots = inventory.GetSlots();

             for (int i = 0; i < size.x; i++)
             {
                 for (int j = 0; j < size.y; j++)
                 {
                     int index = i * size.y + j;
                     InventorySlotView slotView = view.GetInventorySlotsViews(index);
                     IReadOnlyInventorySlot slot = slots[i, j];
                     _slotControllers.Add(new InventoryViewModel(slot, slotView, new AssetSpriteLoader(), new AssetConfigLoader()));
                 }
             }
             
             view.OwnerId = inventory.OwnerId;
        }
    }
}