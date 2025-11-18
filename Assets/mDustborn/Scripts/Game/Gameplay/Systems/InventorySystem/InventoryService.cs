using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryService
    {
        private readonly IGameStateSaver _gameStateSaver;
        
        public readonly Dictionary<string, InventoryGrid> _inventoriesMap = new();

        public InventoryService(IGameStateSaver gameStateSaver)
        {
            _gameStateSaver = gameStateSaver;
        }
        
        public InventoryGrid RegisterInventory(InventoryGridData data)
        {
            var inventory = new InventoryGrid(data);
            _inventoriesMap[inventory.OwnerId] = inventory;
            
            return inventory;
        }
        
        public AddItemsToInventoryGridResult AddItemsToInventory(string ownerId, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            var result = inventory.AddItems(itemId, amount);
            _gameStateSaver.Save();
            return result;
        }
        
        public AddItemsToInventoryGridResult AddItemsToInventory(string ownerId, Vector2Int slootCoords, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            var result = inventory.AddItems(itemId, amount);
            _gameStateSaver.Save();
            return result;
        }
        
        public RemoveItemsToInventoryGridResult RemoveItemsToInventory(string ownerId, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            var result = inventory.RemoveItems(itemId, amount);
            _gameStateSaver.Save();
            return result;
        }
        
        public RemoveItemsToInventoryGridResult RemoveItemsToInventory(string ownerId, Vector2Int slootCoords, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            var result = inventory.RemoveItems(slootCoords, itemId, amount);
            _gameStateSaver.Save();
            return result;
        }

        public bool Has(string ownerId, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            return inventory.Has(itemId, amount);
        }

        public IReadOnlyInventoryGrid GetInventory(string ownerId)
        {
            return _inventoriesMap[ownerId];
        }
    }
}