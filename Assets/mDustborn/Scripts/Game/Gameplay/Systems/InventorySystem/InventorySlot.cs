using R3;

namespace Inventory
{
    public class InventorySlot : IReadOnlyInventorySlot
    {
        private readonly InventorySlotData _data;

        public string ItemId
        {
            get => _data.ItemId;
            set
            {
                if (_data.ItemId != value)
                {
                    _data.ItemId = value;
                    _itemIdChanged.Value = value;
                }
            }
        }
        
        public int Amount
        {
            get => _data.Amount;
            set
            {
                if (_data.Amount != value)
                {
                    _data.Amount = value;
                    _itemAmountChanged.Value = value;
                }
            }
        }

        public bool IsEmpty => Amount == 0 && string.IsNullOrEmpty(ItemId);
        
        private readonly ReactiveProperty<string> _itemIdChanged = new();
        private readonly ReactiveProperty<int> _itemAmountChanged = new();
        
        public Observable<string> ItemIdChanged => _itemIdChanged;
        public Observable<int> ItemAmountChanged => _itemAmountChanged;

        public InventorySlot(InventorySlotData data)
        {
            _data = data;
        }
    }
}