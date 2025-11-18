using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
public class PickupbleInventoryItem : MonoBehaviour
{
    [SerializeField, Expandable] private InventoryItemConfig _inventoryItemConfig;

    public Outline Outline { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    
    public InventoryItemConfig InventoryItemConfig { get; private set; }
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Outline = GetComponent<Outline>();
        Outline.OutlineWidth = 4f;
        
        Color color = Color.red;
        
        if (ColorUtility.TryParseHtmlString("#FF8100", out color))
        {
            Outline.OutlineColor = color;
        }
        
        Outline.enabled = false;
        
        InitializeItem();
    }

    private void InitializeItem()
    {
        InventoryItemConfig = Instantiate(_inventoryItemConfig);
    }
}
