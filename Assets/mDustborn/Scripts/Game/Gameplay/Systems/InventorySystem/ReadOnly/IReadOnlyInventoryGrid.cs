using System;
using UnityEngine;

namespace Inventory
{
    public interface IReadOnlyInventoryGrid : IReadOnlyInventory
    {
        public Vector2Int Size { get; }
        
        public IReadOnlyInventorySlot[,] GetSlots();
        
        public event Action<Vector2Int> SizeChanged;
    }
}