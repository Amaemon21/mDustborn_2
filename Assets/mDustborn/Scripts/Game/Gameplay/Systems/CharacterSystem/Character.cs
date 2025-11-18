using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, IControllable
{
    [SerializeField] private IKHandler _ikHandler;

    [Header("Movement")] 
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _landSecendos = 1;

    [Header("Camera & Player")] 
    [SerializeField] private float _sensitivity = 1f;

    private Vector2 _moveDirection;
    private Vector2 _lookDirection;
    
    private float _velocity;
    private float _xRotation;
    
    private bool _isGrounded;
    private bool _isAiming;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _velocity < 0)
        {
            _velocity = -2f;
        }

        MoveInternal();
        LookInternal();
        DoGravity();
    }
    
    public void Move(Vector2 direction)
    {
        _moveDirection = direction;
    }
    
    public void Look(Vector2 direction)
    {
        _lookDirection = direction;
    }

    private void MoveInternal()
    {
        Vector3 move = _characterController.transform.right * _moveDirection.x + _characterController.transform.forward * _moveDirection.y;
        
        _characterController.Move(move * _moveSpeed * Time.deltaTime);
    }
    
    private void LookInternal()
    {
        Vector3 cameraPosition = -_ikHandler.LocalCameraPoint.position;

        _xRotation -= _lookDirection.y * _sensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.localPosition = transform.localRotation * cameraPosition - cameraPosition;

        _characterController.transform.Rotate(Vector3.up, _lookDirection.x * _sensitivity);
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _velocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            _animator.SetBool(AnimationConstrains.IS_IN_AIR, true);

            StartCoroutine(AfterDelay(OnLand, _landSecendos));
        }
    }

    private void DoGravity()
    {
        _velocity += _gravity * Time.deltaTime;

        _characterController.Move(Vector3.up * _velocity * Time.deltaTime);
    }

    private void OnLand()
    {
        _animator.SetBool(AnimationConstrains.IS_IN_AIR, false);
    }

    public void OnInspect()
    {
        _animator.CrossFade(AnimationConstrains.INSPECT, 0.1f);
    }

    private IEnumerator AfterDelay(Action callback, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
}