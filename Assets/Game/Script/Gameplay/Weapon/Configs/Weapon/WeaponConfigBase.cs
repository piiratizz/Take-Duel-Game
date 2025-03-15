using UnityEngine;

public class WeaponConfigBase : ScriptableObject
{
    public int ClipAmmo;
    public int TotalAmmo;
    public int FireRate;
    public ParticleSystem MuzzleFlash;
    public ParticleSystem PlayerHitEffect;
    public ParticleSystem EnvironmentHitEffect;
    public int PlayerDamage;
    public int EnvironmentDamage;
}