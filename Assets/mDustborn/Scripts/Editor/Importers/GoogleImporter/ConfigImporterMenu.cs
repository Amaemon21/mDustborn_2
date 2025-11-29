using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace GoogleImporter
{
    public class ConfigImporterMenu
    {
        private const string CREADENTIAL_PATH = "dustborm-520bf6b3d5be.json";
        private const string SPREADSHEET_ID = "1Lj5ob2Q4BZnb_Bi92o-aro9yjKrIB_MKsWU5HYY5L_w";
        private const string ITEMS_SHEET_NAME = "InventoryItems";
        private const string ITEM_KEY = "ITEM_SETTINGS";
        
        [MenuItem("Dustborn/Google Importer/Import Items Settings")]
        private static async void LoadItemsSettings()
        {
            GoogleSheetsImporter sheetsImporter = new GoogleSheetsImporter(CREADENTIAL_PATH, SPREADSHEET_ID);

            GameSettings gameSetting = new GameSettings();
            ItemsSettingsParser itemsParser = new ItemsSettingsParser(gameSetting);
            
            await sheetsImporter.DownloadAndParseSheet(ITEMS_SHEET_NAME, itemsParser);
            
            string path = BuildPath(ITEM_KEY);
            string json = JsonConvert.SerializeObject(gameSetting, Formatting.Indented);
            File.WriteAllText(path, json);
        }
        
        private static string BuildPath(string key)
        {
            string saveDirectory = Path.Combine(Application.dataPath, "Resources", "GoogleSheetsJSON");
        
            Directory.CreateDirectory(saveDirectory);
        
            return Path.Combine(saveDirectory, $"{key}.json");
        }
    }
}