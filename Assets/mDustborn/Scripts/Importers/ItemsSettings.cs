using System;

namespace GoogleImporter
{
    [Serializable]
    public class ItemsSettings
    {
        public string Id;
        public ItemType Type;
        public RarityType Rarity;
        public string Name;
        public string Description;
        public string IconName;
        public int Amount;
        public int CellCapacity;
        public string PrefabName;
    }
}