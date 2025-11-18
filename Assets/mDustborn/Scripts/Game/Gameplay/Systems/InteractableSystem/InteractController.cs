using UnityEngine;
using Zenject;

public class InteractController : MonoBehaviour
{
    [Inject] private readonly IInteractHandler _interactHandler;
    
    [SerializeField] private InteractProperty _interactProperty;
    
    private void Awake()
    {
        _interactHandler.SetProperty(_interactProperty);
    }

    private void Update()
    {
        _interactHandler.Interactable();
    }
}