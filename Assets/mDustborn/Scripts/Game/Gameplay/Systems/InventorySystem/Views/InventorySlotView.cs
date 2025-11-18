using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _textTitle;
        [SerializeField] private TMP_Text _textAmount;

        public string Title
        {
            get => _textTitle.text;
            set => _textTitle.text = value;
        }
    
        public int Amount
        {
            get => Convert.ToInt32(_textAmount.text);
            set => _textAmount.text = value == 1 ? String.Empty : value.ToString();
        } 
        
        public Sprite Icon
        {
            get => _icon.sprite;
            set => _icon.sprite = value;
        }
    }
}