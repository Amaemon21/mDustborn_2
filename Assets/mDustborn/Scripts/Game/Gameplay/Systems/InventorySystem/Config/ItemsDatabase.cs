using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsDatabase", menuName = "Inventory System/Items Database")]
public class ItemsDatabase : ScriptableObject
{
    [SerializeField] private List<InventoryItemConfig> _items;
    private Dictionary<string, InventoryItemConfig> _itemsMap;
    
    public InventoryItemConfig GetItem(string id)
    {
        if (_itemsMap == null)
        {
            _itemsMap = new Dictionary<string, InventoryItemConfig>();
            foreach (var item in _items)
                _itemsMap[item.Id] = item;
        }

        return _itemsMap.GetValueOrDefault(id);
    }
}