// Designed by KINEMATION, 2025.

using KINEMATION.FPSAnimationPack.Scripts.Player;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;
using Zenject;

namespace KINEMATION.FPSAnimationPack.Scripts.Camera
{
    [AddComponentMenu("KINEMATION/FPS Animation Pack/FPS Camera Animator")]
    public class FPSCameraAnimator : MonoBehaviour
    {
        [Inject] private readonly WeaponsContainer _weaponsContainer;
        
        [SerializeField] private Transform cameraBone;
        
        private FPSCameraShake _activeShake;
        private Vector3 _cameraShake;
        private Vector3 _cameraShakeTarget;
        private float _cameraShakePlayback;

        private UnityEngine.Camera _camera;
        private IKHandler _ikHandler;
        private float _baseFov;
        
        public virtual void PlayCameraShake(FPSCameraShake newShake)
        {
            if (newShake == null) return;

            _activeShake = newShake;
            _cameraShakePlayback = 0f;

            _cameraShakeTarget.x = FPSCameraShake.GetTarget(_activeShake.pitch);
            _cameraShakeTarget.y = FPSCameraShake.GetTarget(_activeShake.yaw);
            _cameraShakeTarget.z = FPSCameraShake.GetTarget(_activeShake.roll);
        }
        
        protected virtual void UpdateCameraShake()
        {
            if (_activeShake == null) return;

            float length = _activeShake.shakeCurve.GetCurveLength();
            _cameraShakePlayback += Time.deltaTime * _activeShake.playRate;
            _cameraShakePlayback = Mathf.Clamp(_cameraShakePlayback, 0f, length);

            float alpha = KMath.ExpDecayAlpha(_activeShake.smoothSpeed, Time.deltaTime);
            if (!KAnimationMath.IsWeightRelevant(_activeShake.smoothSpeed))
            {
                alpha = 1f;
            }

            Vector3 target = _activeShake.shakeCurve.GetValue(_cameraShakePlayback);
            target.x *= _cameraShakeTarget.x;
            target.y *= _cameraShakeTarget.y;
            target.z *= _cameraShakeTarget.z;
            
            _cameraShake = Vector3.Lerp(_cameraShake, target, alpha);
            transform.rotation *= Quaternion.Euler(_cameraShake);
        }

        protected virtual void UpdateFOV()
        {
            if (_camera == null || _ikHandler == null) return;
            
            _camera.fieldOfView = Mathf.Lerp(_baseFov, _weaponsContainer.CurrentWeapon.Value.weaponSettings.aimFov, _ikHandler.AdsWeight);
        }

        private void Awake()
        {
            _ikHandler = transform.root.GetComponentInChildren<IKHandler>();
            _camera = GetComponent<UnityEngine.Camera>();
            _baseFov = _camera.fieldOfView;
        }

        private void LateUpdate()
        {
            transform.localRotation = _ikHandler.transform.localRotation * cameraBone.localRotation;
            UpdateCameraShake();
            UpdateFOV();
        }
    }
}
