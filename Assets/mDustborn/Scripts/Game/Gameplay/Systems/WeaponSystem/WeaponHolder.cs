using System.Collections.Generic;
using KINEMATION.FPSAnimationPack.Scripts.Weapon;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;
using Zenject;

public class WeaponHolder : MonoBehaviour
{
    [Inject] private readonly WeaponsContainer _weaponsContainer;
    
    [SerializeField] private IKHandler _ikHandler;
    
    private List<FPSWeapon> _weapons = new List<FPSWeapon>();
    private int _activeWeaponIndex = 0;
    
    private void Awake()
    {
        KTransform root = new KTransform(transform);
        KTransform localCameraPoint = root.GetRelativeTransform(new KTransform(_ikHandler.Skeleton.CameraPoint), false);

        foreach (var prefab in _ikHandler.PlayerSettings.weaponPrefabs)
        {
            FPSWeapon prefabComponent = prefab.GetComponent<FPSWeapon>();
            if(prefabComponent == null) continue;
                
            var instance = Instantiate(prefab, _ikHandler.Skeleton.WeaponBone, false);
            instance.SetActive(false);
                
            var component = instance.GetComponent<FPSWeapon>();
            component.Initialize(gameObject);

            KTransform weaponT = new KTransform(_ikHandler.Skeleton.WeaponBone);
            component.rightHandPose = new KTransform(_ikHandler.Skeleton.RightHand.tip).GetRelativeTransform(weaponT, false);
                
            var localWeapon = root.GetRelativeTransform(weaponT, false);

            localWeapon.rotation *= prefabComponent.weaponSettings.rotationOffset;
                
            component.adsPose.position = localCameraPoint.position - localWeapon.position;
            component.adsPose.rotation = Quaternion.Inverse(localWeapon.rotation);

            _weapons.Add(component);
        }
            
        GetActiveWeapon().gameObject.SetActive(true);
        GetActiveWeapon().OnEquipped();
    }
    
    private void EquipWeapon_Incremental()
    {
        GetActiveWeapon().gameObject.SetActive(false);
        _activeWeaponIndex = _activeWeaponIndex + 1 > _weapons.Count - 1 ? 0 : _activeWeaponIndex + 1;
        GetActiveWeapon().OnEquipped();
        Invoke(nameof(SetWeaponVisible), 0.05f);
    }

    private void EquipWeapon()
    {
        GetActiveWeapon().gameObject.SetActive(false);
        GetActiveWeapon().OnEquipped(true);
        Invoke(nameof(SetWeaponVisible), 0.05f);
    }
    
    private FPSWeapon GetActiveWeapon()
    {
        var weapon = _weapons[_activeWeaponIndex];
        _weaponsContainer.Setup(weapon);
        return weapon;
    }
    
    public void OnChangeWeapon()
    {
        if (_weapons.Count <= 1) return;
        float delay = GetActiveWeapon().OnUnEquipped();
        Invoke(nameof(EquipWeapon_Incremental), delay);
    }
    
    private void SetWeaponVisible()
    {
        GetActiveWeapon().gameObject.SetActive(true);
    }

}
