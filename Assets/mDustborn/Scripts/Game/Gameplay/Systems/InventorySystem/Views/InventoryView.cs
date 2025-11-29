using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;

namespace Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ownerText;
        [SerializeField] private InventorySlotView[] _slotViews;

        private CompositeDisposable _disposables = new();

        public void Bind(InventoryGridViewModel viewModel)
        {
            viewModel.OwnerId.BindTo(id => _ownerText.text = id, _disposables);
            
            IReadOnlyList<InventorySlotViewModel> slotVmViewModels = viewModel.SlotViewModels;
            int count = Mathf.Min(_slotViews.Length, slotVmViewModels.Count);

            for (int i = 0; i < count; i++)
            {
                InventorySlotView view = _slotViews[i];
                InventorySlotViewModel slotVmViewModel = slotVmViewModels[i];
                
                view.Bind(slotVmViewModel, _disposables);
            }
        }

        private void OnDisable()
        {
            _disposables?.Dispose();
            _disposables = null;
        }
    }
}