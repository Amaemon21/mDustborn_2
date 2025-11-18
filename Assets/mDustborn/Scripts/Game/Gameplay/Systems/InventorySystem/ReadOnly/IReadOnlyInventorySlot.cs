using R3;

namespace Inventory
{
    public interface IReadOnlyInventorySlot
    {
        string ItemId { get; }
        int Amount { get; }
        bool IsEmpty { get; }
        
        public Observable<string> ItemIdChanged { get; }
        public Observable<int> ItemAmountChanged { get; }
    }
}