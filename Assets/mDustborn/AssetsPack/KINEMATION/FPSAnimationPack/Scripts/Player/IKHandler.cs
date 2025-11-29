using KINEMATION.ProceduralRecoilAnimationSystem.Runtime;
using KINEMATION.KAnimationCore.Runtime.Core;
using KINEMATION.FPSAnimationPack.Scripts.Player;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Zenject;

public class IKHandler : MonoBehaviour
{
    [Inject] private readonly InputService _inputService;
    [Inject] private readonly WeaponsContainer _weaponsContainer;

    [SerializeField] private FPSPlayerSettings _playerSettings;
    [SerializeField] private RecoilAnimation _recoilAnimation;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterSound characterSound;
    
    [Space(10)]
    [SerializeField] private Skeleton _skeleton;
    
    private KTwoBoneIkData _rightHandIk;
    private KTwoBoneIkData _leftHandIk;
    
    private int _tacSprintLayerIndex;
    private int _triggerDisciplineLayerIndex;
    private int _rightHandLayerIndex;

    private bool _isAiming;
    private float _smoothGait;
    private bool _bSprinting;
    private bool _bTacSprinting;

    private float _ikMotionPlayBack;
    private KTransform _ikMotion = KTransform.Identity;
    private KTransform _cachedIkMotion = KTransform.Identity;
    private IKMotion _activeMotion;
    
    public KTransform LocalCameraPoint { get; private set; }
    public float AdsWeight { get; private set; }
    public FPSPlayerSettings PlayerSettings => _playerSettings;
    public Skeleton Skeleton => _skeleton;

    //  private void ThrowGrenade()
    //  {
    //      _weaponModel.CurrentWeapon.Value.gameObject.SetActive(false);
    //      Invoke(nameof(EquipWeapon), playerSettings.grenadeDelay);
    //  }
    //  
    //  public void OnThrowGrenade()
    //  {
    //      _animator.SetTrigger(THROW_GRENADE);
    //      Invoke(nameof(ThrowGrenade), _weaponModel.CurrentWeapon.Value.UnEquipDelay);
    //  }

    public void OnChangeFireMode()
    {
        var prevFireMode = _weaponsContainer.CurrentWeapon.Value.ActiveFireMode;
        _weaponsContainer.CurrentWeapon.Value.OnFireModeChange();

        if (prevFireMode != _weaponsContainer.CurrentWeapon.Value.ActiveFireMode)
        {
            characterSound.PlayFireModeSwitchSound();
            PlayIkMotion(_playerSettings.fireModeMotion);
        }
    }

    public void OnReload()
    {
        _weaponsContainer.CurrentWeapon.Value.OnReload();
    }

    public void Fire(bool isPressed)
    {
        if (isPressed)
        {
            _weaponsContainer.CurrentWeapon.Value.OnFirePressed();
        }
        else
        {
            _weaponsContainer.CurrentWeapon.Value.OnFireReleased();
        }
    }

    private void OnFire()
    {
        _recoilAnimation.Play();
        Debug.Log("Recoil");
    }

    public void SetAim(bool isPressed)
    {
        bool wasAiming = _isAiming;
        _isAiming = isPressed;
        _recoilAnimation.isAiming = _isAiming;

        if (wasAiming != _isAiming)
        {
            characterSound.PlayAimSound(_isAiming);
            PlayIkMotion(_playerSettings.aimingMotion);
        }
    }

    public void SetSprint(bool isPressed)
    {
        _bSprinting = isPressed;
        if (!_bSprinting) _bTacSprinting = false;
    }

    public void SetTacSprint(bool isPressed)
    {
        if (!_bSprinting) return;
        _bTacSprinting = isPressed;
    }

    private void Awake()
    {
        _triggerDisciplineLayerIndex = _animator.GetLayerIndex("TriggerDiscipline");
        _rightHandLayerIndex = _animator.GetLayerIndex("RightHand");
        _tacSprintLayerIndex = _animator.GetLayerIndex("TacSprint");

        KTransform root = new KTransform(transform);
        LocalCameraPoint = root.GetRelativeTransform(new KTransform(_skeleton.CameraPoint), false);
    }

    private void Update()
    {
        AdsWeight = Mathf.Clamp01(AdsWeight + _playerSettings.aimSpeed * Time.deltaTime * (_isAiming ? 1f : -1f));

        _smoothGait = Mathf.Lerp(_smoothGait, GetDesiredGait(),
            KMath.ExpDecayAlpha(_playerSettings.gaitSmoothing, Time.deltaTime));

        _animator.SetFloat(AnimationConstrains.GAIT, _smoothGait);
        _animator.SetLayerWeight(_tacSprintLayerIndex, Mathf.Clamp01(_smoothGait - 2f));

        bool triggerAllowed = _weaponsContainer.CurrentWeapon.Value.weaponSettings.useSprintTriggerDiscipline;

        _animator.SetLayerWeight(_triggerDisciplineLayerIndex,
            triggerAllowed ? _animator.GetFloat(AnimationConstrains.TAC_SPRINT_WEIGHT) : 0f);

        _animator.SetLayerWeight(_rightHandLayerIndex, _animator.GetFloat(AnimationConstrains.RIGHT_HAND_WEIGHT));
    }

    private void LateUpdate()
    {
        KAnimationMath.RotateInSpace(transform, _skeleton.RightHand.tip,
            _weaponsContainer.CurrentWeapon.Value.weaponSettings.rightHandSprintOffset,
            _animator.GetFloat(AnimationConstrains.TAC_SPRINT_WEIGHT));

        KTransform weaponTransform = GetWeaponPose();

        weaponTransform.rotation = KAnimationMath.RotateInSpace(weaponTransform, weaponTransform,
            _weaponsContainer.CurrentWeapon.Value.weaponSettings.rotationOffset, 1f);

        KTransform rightHandTarget = weaponTransform.GetRelativeTransform(new KTransform(_skeleton.RightHand.tip), false);
        KTransform leftHandTarget = weaponTransform.GetRelativeTransform(new KTransform(_skeleton.LeftHand.tip), false);

        ProcessOffsets(ref weaponTransform);
        ProcessAds(ref weaponTransform);
        ProcessAdditives(ref weaponTransform);
        ProcessIkMotion(ref weaponTransform);
        ProcessRecoil(ref weaponTransform);

        _skeleton.WeaponBone.position = weaponTransform.position;
        _skeleton.WeaponBone.rotation = weaponTransform.rotation;

        rightHandTarget = weaponTransform.GetWorldTransform(rightHandTarget, false);
        leftHandTarget = weaponTransform.GetWorldTransform(leftHandTarget, false);

        SetupIkData(ref _rightHandIk, rightHandTarget, _skeleton.RightHand, _playerSettings.ikWeight);
        SetupIkData(ref _leftHandIk, leftHandTarget, _skeleton.LeftHand, _playerSettings.ikWeight);

        KTwoBoneIK.Solve(ref _rightHandIk);
        KTwoBoneIK.Solve(ref _leftHandIk);

        ApplyIkData(_rightHandIk, _skeleton.RightHand);
        ApplyIkData(_leftHandIk, _skeleton.LeftHand);
    }

    private float GetDesiredGait()
    {
        if (_bTacSprinting)
            return 3f;
        if (_bSprinting)
            return 2f;
        return _inputService.MoveDirection.magnitude;
    }

    private void SetupIkData(ref KTwoBoneIkData ikData, in KTransform target, in IKTransforms transforms,
        float weight = 1f)
    {
        ikData.target = target;

        ikData.tip = new KTransform(transforms.tip);
        ikData.mid = ikData.hint = new KTransform(transforms.mid);
        ikData.root = new KTransform(transforms.root);

        ikData.hintWeight = weight;
        ikData.posWeight = weight;
        ikData.rotWeight = weight;
    }

    private void ApplyIkData(in KTwoBoneIkData ikData, in IKTransforms transforms)
    {
        transforms.root.rotation = ikData.root.rotation;
        transforms.mid.rotation = ikData.mid.rotation;
        transforms.tip.rotation = ikData.tip.rotation;
    }

    private void ProcessOffsets(ref KTransform weaponT)
    {
        var root = transform;
        KTransform rootT = new KTransform(root);
        var weaponOffset = _weaponsContainer.CurrentWeapon.Value.weaponSettings.ikOffset;

        float mask = 1f - _animator.GetFloat(AnimationConstrains.TAC_SPRINT_WEIGHT);
        weaponT.position = KAnimationMath.MoveInSpace(rootT, weaponT, weaponOffset, mask);

        var settings = _weaponsContainer.CurrentWeapon.Value.weaponSettings;
        KAnimationMath.MoveInSpace(root, _skeleton.RightHand.root, settings.rightClavicleOffset, mask);
        KAnimationMath.MoveInSpace(root, _skeleton.LeftHand.root, settings.leftClavicleOffset, mask);
    }

    private void ProcessAdditives(ref KTransform weaponT)
    {
        KTransform rootT = new KTransform(_skeleton.SkeletonRoot);
        KTransform additive = rootT.GetRelativeTransform(new KTransform(_skeleton.WeaponBoneAdditive), false);

        float weight = Mathf.Lerp(1f, 0.3f, AdsWeight) * (1f - _animator.GetFloat(AnimationConstrains.GRENADE_WEIGHT));

        weaponT.position = KAnimationMath.MoveInSpace(rootT, weaponT, additive.position, weight);
        weaponT.rotation = KAnimationMath.RotateInSpace(rootT, weaponT, additive.rotation, weight);
    }

    private void ProcessRecoil(ref KTransform weaponT)
    {
        KTransform recoil = new KTransform()
        {
            rotation = _recoilAnimation.OutRot,
            position = _recoilAnimation.OutLoc,
        };

        KTransform root = new KTransform(transform);
        weaponT.position = KAnimationMath.MoveInSpace(root, weaponT, recoil.position, 1f);
        weaponT.rotation = KAnimationMath.RotateInSpace(root, weaponT, recoil.rotation, 1f);
    }

    private void ProcessAds(ref KTransform weaponT)
    {
        var weaponOffset = _weaponsContainer.CurrentWeapon.Value.weaponSettings.ikOffset;
        var adsPose = weaponT;

        KTransform aimPoint = KTransform.Identity;

        aimPoint.position = -_skeleton.WeaponBone.InverseTransformPoint(_weaponsContainer.CurrentWeapon.Value.aimPoint.position);
        aimPoint.position -= _weaponsContainer.CurrentWeapon.Value.weaponSettings.aimPointOffset;
        aimPoint.rotation =
            Quaternion.Inverse(_skeleton.WeaponBone.rotation) * _weaponsContainer.CurrentWeapon.Value.aimPoint.rotation;

        KTransform root = new KTransform(transform);
        adsPose.position = KAnimationMath.MoveInSpace(root, adsPose,
            _weaponsContainer.CurrentWeapon.Value.adsPose.position - weaponOffset, 1f);
        adsPose.rotation =
            KAnimationMath.RotateInSpace(root, adsPose, _weaponsContainer.CurrentWeapon.Value.adsPose.rotation, 1f);

        KTransform cameraPose = root.GetWorldTransform(LocalCameraPoint, false);

        float adsBlendWeight = _weaponsContainer.CurrentWeapon.Value.weaponSettings.adsBlend;
        adsPose.position = Vector3.Lerp(cameraPose.position, adsPose.position, adsBlendWeight);
        adsPose.rotation = Quaternion.Slerp(cameraPose.rotation, adsPose.rotation, adsBlendWeight);

        adsPose.position = KAnimationMath.MoveInSpace(root, adsPose, aimPoint.rotation * aimPoint.position, 1f);
        adsPose.rotation = KAnimationMath.RotateInSpace(root, adsPose, aimPoint.rotation, 1f);

        float weight = KCurves.EaseSine(0f, 1f, AdsWeight);

        weaponT.position = Vector3.Lerp(weaponT.position, adsPose.position, weight);
        weaponT.rotation = Quaternion.Slerp(weaponT.rotation, adsPose.rotation, weight);
    }

    private KTransform GetWeaponPose()
    {
        KTransform defaultWorldPose =
            new KTransform(_skeleton.RightHand.tip).GetWorldTransform(_weaponsContainer.CurrentWeapon.Value.rightHandPose, false);
        float weight = _animator.GetFloat(AnimationConstrains.RIGHT_HAND_WEIGHT);

        return KTransform.Lerp(new KTransform(_skeleton.WeaponBone), defaultWorldPose, weight);
    }

    private void PlayIkMotion(IKMotion newMotion)
    {
        _ikMotionPlayBack = 0f;
        _cachedIkMotion = _ikMotion;
        _activeMotion = newMotion;
    }

    private void ProcessIkMotion(ref KTransform weaponT)
    {
        if (_activeMotion == null) return;

        _ikMotionPlayBack = Mathf.Clamp(_ikMotionPlayBack + _activeMotion.playRate * Time.deltaTime, 0f,
            _activeMotion.GetLength());

        Vector3 positionTarget = _activeMotion.translationCurves.GetValue(_ikMotionPlayBack);
        positionTarget.x *= _activeMotion.translationScale.x;
        positionTarget.y *= _activeMotion.translationScale.y;
        positionTarget.z *= _activeMotion.translationScale.z;

        Vector3 rotationTarget = _activeMotion.rotationCurves.GetValue(_ikMotionPlayBack);
        rotationTarget.x *= _activeMotion.rotationScale.x;
        rotationTarget.y *= _activeMotion.rotationScale.y;
        rotationTarget.z *= _activeMotion.rotationScale.z;

        _ikMotion.position = positionTarget;
        _ikMotion.rotation = Quaternion.Euler(rotationTarget);

        if (!Mathf.Approximately(_activeMotion.blendTime, 0f))
        {
            _ikMotion = KTransform.Lerp(_cachedIkMotion, _ikMotion, _ikMotionPlayBack / _activeMotion.blendTime);
        }

        var root = new KTransform(transform);
        weaponT.position = KAnimationMath.MoveInSpace(root, weaponT, _ikMotion.position, 1f);
        weaponT.rotation = KAnimationMath.RotateInSpace(root, weaponT, _ikMotion.rotation, 1f);
    }
}