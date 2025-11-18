using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetSpriteLoader : LocalAssetLoader<Sprite>
{
    protected override AsyncOperationHandle<Sprite> CreateLoadOperation(string assetId)
    {
        return Addressables.LoadAssetAsync<Sprite>(assetId);
    }
    
    public void LoadSprite(string spriteId, Action<Sprite> onLoaded)
    {
        _ = LoadSpriteInternal(spriteId, onLoaded);
    }
    
    private async Task LoadSpriteInternal(string spriteId, Action<Sprite> onLoaded)
    {
        Release();

        var sprite = await LoadAsync(spriteId);
        onLoaded?.Invoke(sprite);
    }
}