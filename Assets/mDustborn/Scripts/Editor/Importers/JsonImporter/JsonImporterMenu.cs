using UnityEditor;

namespace JsonImporterToSO
{
    public class JsonImporterMenu
    {
        [MenuItem("Dustborn/JSON Importer/Import Items Config")]
        private static void LoadItemConfig()
        {
            InventoryItemImporterProfile profile = new InventoryItemImporterProfile();
            
            JsonImportRunner.Run(profile);
        }
    }
}