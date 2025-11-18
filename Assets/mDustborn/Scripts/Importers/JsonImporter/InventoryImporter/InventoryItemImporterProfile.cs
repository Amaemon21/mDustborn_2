using GoogleImporter;
using Newtonsoft.Json;

namespace JsonImporterToSO
{
    public class InventoryItemImporterProfile : IJsonImportProfile
    {
        private const string RESOURCES_FOLDER = "GoogleSheetsJSON";
        private const string ITEM_KEY = "ITEM_SETTINGS";
        private const string OUTPUT_FOLDER_CONST = "Assets/Resources/Configs/Items";

        public string ResourcesPath => $"{RESOURCES_FOLDER}/{ITEM_KEY}";
        public string OutputFolder => OUTPUT_FOLDER_CONST;

        public void Import(string json)
        {
            GameSettings gameSettings = JsonConvert.DeserializeObject<GameSettings>(json);

            var importer = new JsonInventoryItemImporter(OutputFolder, gameSettings);
            importer.ImportItems();
        }
    }
}