using UnityEditor;
using UnityEngine;

namespace JsonImporterToSO
{
    public class JsonImportRunner
    {
        public static void Run(IJsonImportProfile profile)
        {
            string resourcesPath = profile.ResourcesPath;

            TextAsset jsonAsset = Resources.Load<TextAsset>(resourcesPath);

            profile.Import(jsonAsset.text);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"JsonImportRunner: импорт завершён. OutputFolder = {profile.OutputFolder}");
        }
    }
}