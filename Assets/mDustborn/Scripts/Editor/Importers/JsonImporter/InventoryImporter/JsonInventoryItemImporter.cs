using System.Reflection;
using GoogleImporter;
using UnityEditor;
using UnityEngine;

namespace JsonImporterToSO
{
    public class JsonInventoryItemImporter
    {
        private readonly string _outputFolderPath;
        private readonly GameSettings _settings;

        public JsonInventoryItemImporter(string outputFolderPath, GameSettings settings)
        {
            _outputFolderPath = outputFolderPath;
            _settings = settings;
        }

        public void ImportItems()
        {
            EnsureFolders();
            
            foreach (ItemsSettings itemSettings in _settings.Items)
            {
                CreateOrUpdateItemAsset(itemSettings);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"ItemsConfigFromJsonImporter: импортировано {_settings.Items.Count} предметов в {_outputFolderPath}.");
        }
        
        private void EnsureFolders()
        {
            var path = _outputFolderPath.Replace("\\", "/");

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("JsonImporter.EnsureFolders: пустой _outputFolderPath");
                return;
            }

            if (!path.StartsWith("Assets"))
            {
                Debug.LogError($"JsonImporter.EnsureFolders: путь должен начинаться с 'Assets', сейчас: {path}");
                return;
            }
            
            var parts = path.Split('/');
            string current = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string parent = current;
                string folderName = parts[i];
                string fullPath = parent + "/" + folderName;

                if (!AssetDatabase.IsValidFolder(fullPath))
                {
                    AssetDatabase.CreateFolder(parent, folderName);
                }

                current = fullPath;
            }
        }

        private void CreateOrUpdateItemAsset(ItemsSettings itemSettings)
        {
            string assetPath = $"{_outputFolderPath}/{itemSettings.Id}.asset";

            System.Type soType = itemSettings.Type switch
            {
                ItemType.Weapon      => typeof(WeaponItemConfig),
                ItemType.Ammo        => typeof(AmmoItemConfig),
                ItemType.Scope       => typeof(ScopeItemConfig),
                ItemType.Medications => typeof(MedicationItemConfig),
                ItemType.Default       => typeof(DefaultItemConfig),
                _ => throw new System.Exception($"Unknown ItemType: {itemSettings.Type}")
            };

            var asset = AssetDatabase.LoadAssetAtPath(assetPath, soType) as InventoryItemConfig;

            bool isNew = false;

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance(soType) as InventoryItemConfig;
                isNew = true;
            }

            SetPropertyOrBackingField(asset, "Id",              itemSettings.Id);
            SetPropertyOrBackingField(asset, "ItemType",        itemSettings.Type);
            SetPropertyOrBackingField(asset, "ItemRarity",      itemSettings.Rarity);
            SetPropertyOrBackingField(asset, "ItemName",        itemSettings.Name);
            SetPropertyOrBackingField(asset, "ItemDescription", itemSettings.Description);
            SetPropertyOrBackingField(asset, "IconName",        itemSettings.IconName);
            SetPropertyOrBackingField(asset, "Amount",          itemSettings.Amount);
            SetPropertyOrBackingField(asset, "CellCapacity",    itemSettings.CellCapacity);
            SetPropertyOrBackingField(asset, "PrefabName",      itemSettings.PrefabName);

            if (isNew)
                AssetDatabase.CreateAsset(asset, assetPath);

            EditorUtility.SetDirty(asset);
        }

        private void SetPropertyOrBackingField(object target, string propertyName, object value)
        {
            var type = target.GetType();

            // 1. Пытаемся найти ПРОПЕРТИ по всей иерархии (InventoryItemConfig -> WeaponItemConfig и т.п.)
            System.Type t = type;
            while (t != null)
            {
                var prop = t.GetProperty(propertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                if (prop != null)
                {
                    var setter = prop.GetSetMethod(true); // true – достаём и private-set
                    if (setter != null)
                    {
                        setter.Invoke(target, new[] { value });
                        return;
                    }
                }

                t = t.BaseType;
            }

            // 2. Если setter не нашли – ищем backing field <PropertyName>k__BackingField по всей иерархии
            t = type;
            string backingFieldName = $"<{propertyName}>k__BackingField";

            while (t != null)
            {
                var field = t.GetField(backingFieldName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                if (field != null)
                {
                    field.SetValue(target, value);
                    return;
                }

                t = t.BaseType;
            }

            // 3. На всякий случай – обычное поле с таким именем (если где-то не auto-property)
            t = type;
            while (t != null)
            {
                var field = t.GetField(propertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                if (field != null)
                {
                    field.SetValue(target, value);
                    return;
                }

                t = t.BaseType;
            }

            Debug.LogWarning($"ItemsConfigFromJsonImporter: не удалось установить '{propertyName}' на {type.Name}");
        }
    }
}