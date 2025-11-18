using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class LocalAssetLoader<T> : IDisposable
{
    protected AsyncOperationHandle<T>? _handle;
    
    protected virtual AsyncOperationHandle<T> CreateLoadOperation(string assetId)
    {
        return Addressables.LoadAssetAsync<T>(assetId);
    }
    
    public async Task<T> LoadAsync(string assetId)
    {
        if (_handle.HasValue)
        {
            Addressables.Release(_handle.Value);
            _handle = null;
        }

        _handle = CreateLoadOperation(assetId);
        await _handle.Value.Task;
        return _handle.Value.Result;
    }
    
    public virtual void Release()
    {
        if (_handle.HasValue)
        {
            Addressables.Release(_handle.Value);
            _handle = null;
        }
    }

    public virtual void Dispose()
    {
        Release();
    }
}