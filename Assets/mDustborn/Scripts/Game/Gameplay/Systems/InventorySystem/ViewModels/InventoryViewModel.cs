using R3;
using UnityEngine;

namespace Inventory
{
    public class InventoryViewModel
    {
        private readonly AssetSpriteLoader _assetSpriteLoader;
        private readonly AssetConfigLoader _assetConfigLoader;
        private readonly InventorySlotView _view;
        private readonly IReadOnlyInventorySlot _slot;

        private InventoryItemConfig _inventoryItemConfig;
        
        private int _iconRequestId;

        private ReactiveProperty<string> _itemName;
        private ReactiveProperty<int> _amount;
        private ReactiveProperty<Sprite> _itemIcon;

        public InventoryViewModel(IReadOnlyInventorySlot slot, InventorySlotView view, AssetSpriteLoader assetSpriteLoader, AssetConfigLoader assetConfigLoader)
        {
            _assetSpriteLoader = assetSpriteLoader;
            _assetConfigLoader = assetConfigLoader;
            
            _view = view;
            _slot = slot;

            _slot.ItemIdChanged.Subscribe(SlotOnItemIdChanged);
            _slot.ItemAmountChanged.Subscribe(SlotOnItemAmountChanged);

            _view.Title = _slot.ItemId;
            _view.Amount = _slot.Amount;

            UpdateIcon(_slot.ItemId);
        }

        private void SlotOnItemIdChanged(string newItemId)
        {
            _view.Title = newItemId;
            UpdateIcon(newItemId);
        }

        private void SlotOnItemAmountChanged(int newAmount)
        {
            _view.Amount = newAmount;
        }

        private void UpdateIcon(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                _inventoryItemConfig = null;
                _view.Icon = null;
                return;
            }

            int requestId = ++_iconRequestId;
            
            _assetConfigLoader.LoadConfig(itemId, configObj =>
            {
                if (requestId != _iconRequestId)
                    return;

                var config = configObj as InventoryItemConfig;
                _inventoryItemConfig = config;

                if (config == null || string.IsNullOrEmpty(config.IconName))
                {
                    _view.Icon = null;
                    return;
                }
                
                _assetSpriteLoader.LoadSprite(config.IconName, sprite =>
                {
                    if (requestId != _iconRequestId)
                        return;

                    _view.Icon = sprite;
                });
            });
        }
    }
}