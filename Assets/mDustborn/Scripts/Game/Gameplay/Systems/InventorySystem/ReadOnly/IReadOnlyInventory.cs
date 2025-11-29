using R3;

namespace Inventory
{
    public interface IReadOnlyInventory
    {
        Observable<(string itemId, int amount)> ItemsAdded { get; }
        Observable<(string itemId, int amount)> ItemsRemoved { get; }
        
        public string OwnerId { get; }
        public int GetAmount(string itemId);
        bool Has(string itemId, int amount);
    }
}