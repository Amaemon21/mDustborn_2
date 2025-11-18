using R3;
using UnityEngine;
using Zenject;

public class CharacterInputController : MonoBehaviour
{
    [Inject] private readonly InputService _inputService;
    
    private IControllable _controllable;
    private CompositeDisposable _disposables = new();

    private void Awake()
    {
        _controllable = GetComponent<IControllable>();

        if (_controllable == null)
        {
            throw new MissingComponentException($"There is no IControllable component on the object: {gameObject.name}");
        }
    }

    private void OnEnable()
    {
        _inputService.JumpPressed.Subscribe(_ => ReadJump()).AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables?.Dispose();
    }

    private void Update()
    {
        ReadMove();
        ReadLook();
    }

    private void ReadMove()
    {
        Vector2 direction = _inputService.MoveDirection;
        
        _controllable.Move(direction);
    }

    private void ReadLook()
    {
        Vector2 direction = _inputService.LookDirection;
        
        _controllable.Look(direction);
    }
    
    private void ReadJump()
    {
        _controllable.Jump();
    }
}