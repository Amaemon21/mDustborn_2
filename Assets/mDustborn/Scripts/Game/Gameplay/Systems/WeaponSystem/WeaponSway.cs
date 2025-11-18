using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;
using Zenject;

public class WeaponSway : MonoBehaviour
{
    [Inject] private readonly InputService _inputService;

    [Header("Rotation Sway Settings")]
    [SerializeField] private float _swayAmount = 4f;
    [SerializeField] private float _interpolationSpeed = 5f;
    [SerializeField] private float _maxSwayX = 10f;
    [SerializeField] private float _minSwayX = -10f;
    [SerializeField] private float _maxSwayY = 10f;
    [SerializeField] private float _minSwayY = -10f;

    private Quaternion _swayRotation = Quaternion.identity;

    public void ResetSway()
    {
        _swayRotation = Quaternion.identity;
    }

    public void ProcessSway(ref KTransform transform)
    {
        Vector2 look = _inputService.LookDirection;

        float mouseX = look.x * _swayAmount;
        float mouseY = look.y * _swayAmount;

        mouseX = Mathf.Clamp(mouseX, _minSwayX, _maxSwayX);
        mouseY = Mathf.Clamp(mouseY, _minSwayY, _maxSwayY);

        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.back);
        Quaternion targetRotation = rotationX * rotationY;

        float alphaRot = KMath.ExpDecayAlpha(_interpolationSpeed, Time.deltaTime);
        _swayRotation = Quaternion.Slerp(_swayRotation, targetRotation, alphaRot);

        transform.rotation *= _swayRotation;
    }
}