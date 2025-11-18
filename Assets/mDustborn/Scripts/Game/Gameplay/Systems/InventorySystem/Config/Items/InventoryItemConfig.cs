using NaughtyAttributes;
using UnityEngine;

public abstract class InventoryItemConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Main config"), HorizontalLine] public string Id { get; private set; }
    [field: SerializeField, BoxGroup("Main config"), HorizontalLine] public ItemType ItemType { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public RarityType ItemRarity { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public string ItemName { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public string ItemDescription { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public string IconName { get; private set; }
    [field: SerializeField, BoxGroup("Main config"), MinValue(1)] public int Amount { get; private set; }
    [field: SerializeField, BoxGroup("Main config"), MinValue(1)] public int CellCapacity { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public string PrefabName { get; private set; }
}