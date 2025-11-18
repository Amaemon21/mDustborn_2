using KINEMATION.FPSAnimationPack.Scripts.Weapon;
using NaughtyAttributes;
using UnityEngine;

public class WeaponEffects : MonoBehaviour
{
    [SerializeField, BoxGroup("Muzzle Flash"), HorizontalLine] private GameObject[] _muzzlePrefabs;
    [SerializeField, BoxGroup("Muzzle Flash")] private bool _enableMuzzle = true;
    [SerializeField][Range(0.0f, 2.0f), BoxGroup("Muzzle Flash")] private float _scaleFactor = 1.0f;
    [SerializeField][Range(0.0f, 5.0f), BoxGroup("Muzzle Flash")] private float _destroyTime = 2.0f;
    
    [SerializeField, BoxGroup("Transform"), HorizontalLine] private Transform _barrelTransform;
    
    private FPSWeapon _weapon;

    private void Awake()
    {
        _weapon = GetComponent<FPSWeapon>();
    }

    private void OnEnable()
    {
        //_weapon.OnShootChanged += CreateMuzzleFlash;
    }

    private void OnDisable()
    {
        //_weapon.OnShootChanged -= CreateMuzzleFlash;
    }
    
    private void CreateMuzzleFlash()
    {
        WeaponUtilities.CreateMuzzleFlash(_enableMuzzle, _muzzlePrefabs, _barrelTransform, _scaleFactor, _destroyTime);
    }
}
