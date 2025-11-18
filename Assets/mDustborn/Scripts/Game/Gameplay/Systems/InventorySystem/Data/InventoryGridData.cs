using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class InventoryGridData
    {
        public string OwnerId;
        public List<InventorySlotData> Slots;
        public Vector2Int Size;
    }
}