using Inventory;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _amountText;

    public void Bind(InventorySlotViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.Amount.BindTo(value =>
        {
            _amountText.text = value > 0 ? value.ToString() : string.Empty;
        }, disposables);
        
        viewModel.Icon.BindTo(sprite =>
        {
            _icon.sprite = sprite;
            _icon.enabled = sprite != null;
        }, disposables);
    }
}