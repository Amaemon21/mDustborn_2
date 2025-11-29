using System;
using R3;

public abstract  class ViewModel : IDisposable
{
    protected CompositeDisposable Disposables { get; } = new();
    
    public void Dispose()
    {
        OnDispose();
        Disposables?.Dispose();
    }
    
    protected virtual void OnDispose() { }
}
