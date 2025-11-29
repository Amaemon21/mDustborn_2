using Inventory;
using R3;
using UnityEngine;

public class InventoryScreenView : View<InventoryScreenViewModel>
{
    [SerializeField] private InventoryView _inventoryView;
    
    public InventoryView InventoryView => _inventoryView;
    
    protected override void Bind(InventoryScreenViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.CurrentInventory
            .Where(vm => vm != null)
            .Subscribe(gridVm =>
            {
                _inventoryView.Bind(gridVm);
            })
            .AddTo(disposables);
    }
}