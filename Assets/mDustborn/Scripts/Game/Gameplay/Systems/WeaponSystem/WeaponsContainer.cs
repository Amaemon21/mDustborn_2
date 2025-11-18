using KINEMATION.FPSAnimationPack.Scripts.Weapon;
using R3;

public class WeaponsContainer
{
    public ReactiveProperty<FPSWeapon> CurrentWeapon {get; private set;}

    public WeaponsContainer()
    {
        CurrentWeapon = new();
    }
    
    public void Setup(FPSWeapon weapon)
    {
        CurrentWeapon.Value = weapon;
    }
}