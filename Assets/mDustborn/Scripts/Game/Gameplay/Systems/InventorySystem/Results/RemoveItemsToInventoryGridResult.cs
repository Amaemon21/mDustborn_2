namespace Inventory
{
    public readonly struct RemoveItemsToInventoryGridResult
    {
        public readonly string InventoryOwnerID;
        public readonly int ItemsToRemoveAmount;
        public readonly bool Success;

        public RemoveItemsToInventoryGridResult(string inventoryOwnerID, int itemsToRemoveAmount, bool success)
        {
            InventoryOwnerID = inventoryOwnerID;    
            ItemsToRemoveAmount = itemsToRemoveAmount;
            Success = success;
        }
    }
}