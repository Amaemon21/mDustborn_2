using R3;
using UnityEngine;
using Zenject;

public abstract class View<TViewModel> : MonoBehaviour where TViewModel : ViewModel
{
    protected TViewModel ViewModel { get; private set; }
    
    private CompositeDisposable _viewDisposables = new();
    
    [Inject]
    public void Construct(TViewModel viewModel)
    {
        ViewModel = viewModel;
        OnViewModelInjected(viewModel);
    }
    
    protected virtual void OnViewModelInjected(TViewModel viewModel) { }
    
    protected virtual void OnEnable()
    {
        if (ViewModel == null)
        {
            Debug.LogError($"{name}: ViewModel is null, check Zenject bindings.");
            return;
        }
        
        Bind(ViewModel, _viewDisposables);
    }
    
    protected virtual void OnDestroy()
    {
        _viewDisposables?.Dispose();
        _viewDisposables = null;
    }
    
    protected abstract void Bind(TViewModel viewModel, CompositeDisposable disposables);
}