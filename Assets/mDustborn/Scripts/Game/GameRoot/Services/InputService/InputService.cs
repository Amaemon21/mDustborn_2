using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IDisposable
{
    private readonly GameInputs _gameInputs;

    public Vector2 MoveDirection { get; private set; }
    public Vector2 LookDirection { get; private set; }
    
    public Subject<Unit> JumpPressed = new();
    public Subject<Unit> InteractPressed = new();
    
    public Subject<Unit> InventoryPressed = new();

    public event Action<bool> FireChanged;
    public event Action<bool> AimChanged;
    public event Action<bool> SprintChanged;
    public event Action<bool> TacSprintChanged;

    public event Action ReloadPressed;
    public event Action ChangeWeaponPressed;
    public event Action ChangeFireModePressed;
    public event Action ThrowGrenadePressed;
    public event Action InspectPressed;

    public InputService()
    {
        _gameInputs = new GameInputs();
        _gameInputs.Enable();
        
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled  += OnMove;
        
        _gameInputs.Player.Look.performed += OnLook;
        _gameInputs.Player.Look.canceled  += OnLook;
        
        _gameInputs.Player.Jump.performed += ctx => JumpPressed.OnNext(Unit.Default);
        
        _gameInputs.Player.Fire.performed += ctx => FireChanged?.Invoke(true);
        _gameInputs.Player.Fire.canceled  += ctx => FireChanged?.Invoke(false);
        
        _gameInputs.Player.Aim.performed  += ctx => AimChanged?.Invoke(true);
        _gameInputs.Player.Aim.canceled   += ctx => AimChanged?.Invoke(false);
        
        _gameInputs.Player.Sprint.performed += ctx => SprintChanged?.Invoke(true);
        _gameInputs.Player.Sprint.canceled  += ctx => SprintChanged?.Invoke(false);
        
        _gameInputs.Player.TacSprint.performed += ctx => TacSprintChanged?.Invoke(true);
        _gameInputs.Player.TacSprint.canceled  += ctx => TacSprintChanged?.Invoke(false);
        
        _gameInputs.Player.Reload.performed += ctx => ReloadPressed?.Invoke();
        
        _gameInputs.Player.ChangeWeapon.performed += ctx => ChangeWeaponPressed?.Invoke();
        
        _gameInputs.Player.ChangeFireMode.performed += ctx => ChangeFireModePressed?.Invoke();
        
        _gameInputs.Player.ThrowGrenade.performed += ctx => ThrowGrenadePressed?.Invoke();
        
        _gameInputs.Player.Inspect.performed += ctx => InspectPressed?.Invoke();
        
        _gameInputs.UI.Inventory.performed += ctx => InventoryPressed.OnNext(Unit.Default);
    }
    
    public void EnablePlayerMap() => _gameInputs.Player.Enable();
    public void DisablePlayerMap() => _gameInputs.Player.Disable();

    private void OnMove(InputAction.CallbackContext ctx) => MoveDirection = ctx.ReadValue<Vector2>();

    private void OnLook(InputAction.CallbackContext ctx) => LookDirection = ctx.ReadValue<Vector2>();

    public void Dispose()
    {
        _gameInputs.Player.Move.performed -= OnMove;
        _gameInputs.Player.Move.canceled  -= OnMove;

        _gameInputs.Player.Look.performed -= OnLook;
        _gameInputs.Player.Look.canceled  -= OnLook;

        _gameInputs.Disable();
        _gameInputs.Dispose();
    }
}
