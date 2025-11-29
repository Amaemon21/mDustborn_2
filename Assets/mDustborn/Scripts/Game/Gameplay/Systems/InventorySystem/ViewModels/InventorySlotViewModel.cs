using R3;
using UnityEngine;

namespace Inventory
{
    public class InventorySlotViewModel : ViewModel
    {
        private readonly IReadOnlyInventorySlot _slot;
        private readonly AssetConfigLoader _configLoader;
        private readonly AssetSpriteLoader _spriteLoader;

        public ReactiveProperty<string> ItemId { get; } = new();
        public ReactiveProperty<string> ItemName { get; } = new();
        public ReactiveProperty<int> Amount { get; } = new();
        public ReactiveProperty<Sprite> Icon { get; } = new();
        public ReactiveProperty<bool> IsEmpty { get; } = new();

        public InventorySlotViewModel(IReadOnlyInventorySlot slot, AssetConfigLoader configLoader, AssetSpriteLoader spriteLoader)
        {
            _slot = slot;
            _configLoader = configLoader;
            _spriteLoader = spriteLoader;

            ItemId.Value = slot.ItemId;
            Amount.Value = slot.Amount;
            IsEmpty.Value = slot.IsEmpty;
            
            if (!string.IsNullOrEmpty(slot.ItemId))
            {
                LoadConfigAndIcon(slot.ItemId);
            }
            
            Disposables.Add(_slot.ItemIdChanged.Subscribe(OnItemIdChanged));
            
            Disposables.Add(_slot.ItemAmountChanged.Subscribe(amount =>
                {
                    Amount.Value = amount;
                    IsEmpty.Value = _slot.IsEmpty;
                }));
        }

        private void OnItemIdChanged(string newItemId)
        {
            ItemId.Value = newItemId;
            IsEmpty.Value = string.IsNullOrEmpty(newItemId);
            
            if (IsEmpty.Value)
            {
                ItemName.Value = string.Empty;
                Icon.Value = null;
                return;
            }

            LoadConfigAndIcon(newItemId);
        }
        
        private void LoadConfigAndIcon(string configId)
        {
            _configLoader.LoadConfig(configId, configObject =>
            {
                if (configObject is not InventoryItemConfig config)
                {
                    Debug.LogError($"Config with id {configId} is not InventoryItemConfig");
                    return;
                }

                ItemName.Value = config.ItemName ?? config.Id;

                if (string.IsNullOrEmpty(config.IconName))
                {
                    Icon.Value = null;
                    return;
                }

                _spriteLoader.LoadSprite(config.IconName, sprite =>
                {
                    Icon.Value = sprite;
                });
            });
        }
    }
}