using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetConfigLoader : LocalAssetLoader<ScriptableObject>
{
    protected override AsyncOperationHandle<ScriptableObject> CreateLoadOperation(string assetId)
    {
        return Addressables.LoadAssetAsync<ScriptableObject>(assetId);
    }
    
    public void LoadConfig(string configId, Action<ScriptableObject> onLoaded)
    {
        _ = LoadConfigInternal(configId, onLoaded);
    }
    
    private async Task<ScriptableObject> LoadConfigInternal(string spriteId, Action<ScriptableObject> onLoaded)
    {
        Release();

        var config = await LoadAsync(spriteId);
        onLoaded?.Invoke(config);
        return config;
    }
}