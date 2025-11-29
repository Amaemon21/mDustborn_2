using R3;
using UnityEngine;

namespace Inventory
{
    public interface IReadOnlyInventoryGrid : IReadOnlyInventory
    {
        public Vector2Int Size { get; }
        
        public IReadOnlyInventorySlot[,] GetSlots();
        
        Observable<Vector2Int> SizeChanged { get; }
    }
}